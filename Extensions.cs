using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Capture;
using Windows.Graphics.Imaging;
using BitmapEncoder = Windows.Graphics.Imaging.BitmapEncoder;

namespace WindowsCapture;

public static class Extensions
{
    public static Bitmap ToBitmap(this Direct3D11CaptureFrame frame)
    {
        var texture2d_bitmap = Direct3D11Helper.CreateSharpDXTexture2D(frame.Surface);

        var d3dDevice = texture2d_bitmap.Device;

        // Create texture copy
        var staging = new Texture2D(d3dDevice, new Texture2DDescription
        {
            Width = frame.ContentSize.Width,
            Height = frame.ContentSize.Height,
            MipLevels = 1,
            ArraySize = 1,
            Format = texture2d_bitmap.Description.Format,
            Usage = ResourceUsage.Staging,
            SampleDescription = new SampleDescription(1, 0),
            BindFlags = BindFlags.None,
            CpuAccessFlags = CpuAccessFlags.Read,
            OptionFlags = ResourceOptionFlags.None
        });

        try
        {
            // Copy data
            d3dDevice.ImmediateContext.CopyResource(texture2d_bitmap, staging);

            var dataBox = d3dDevice.ImmediateContext.MapSubresource(staging, 0, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None,
                 out DataStream stream);

            var bitmap = new System.Drawing.Bitmap(staging.Description.Width, staging.Description.Height, dataBox.RowPitch,
                 System.Drawing.Imaging.PixelFormat.Format32bppArgb, dataBox.DataPointer);

            return bitmap;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            staging.Dispose();
        }
    }

    public static Stream ToBitmapStream(this Direct3D11CaptureFrame frame)
    {
        var bitmap = frame.ToBitmap();
        Stream memoryStream = new MemoryStream();
        bitmap.Save(memoryStream, ImageFormat.Png);

        return memoryStream;
    }

    public static async Task<SoftwareBitmap> ToSoftwareBitmapAsync(this Direct3D11CaptureFrame frame)
    {
        var result = await SoftwareBitmap.CreateCopyFromSurfaceAsync(frame.Surface, BitmapAlphaMode.Premultiplied);

        return result;
    }

    public static async Task<Bitmap> ToBitmapAsync(this SoftwareBitmap sbitmap)
    {
        using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
        {
            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
            encoder.SetSoftwareBitmap(sbitmap);
            await encoder.FlushAsync();
            var bmp = new System.Drawing.Bitmap(stream.AsStream());
            return bmp;
        }
    }
}
