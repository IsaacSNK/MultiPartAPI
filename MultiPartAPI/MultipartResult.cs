using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace MultiPartAPI
{
    public class MultipartContent
    {
        public string? ContentType { get; set; }

        public string? FileName { get; set; }

        public Stream? Stream { get; set; }
    }

    public class MultipartResult : Collection<MultipartContent>, IActionResult
    {
        private readonly System.Net.Http.MultipartContent content;

        public MultipartResult(string subtype = "form-data", string boundary = null)
        {
            if (boundary == null)
            {
                this.content = new System.Net.Http.MultipartContent(subtype);
            }
            else
            {
                this.content = new System.Net.Http.MultipartContent(subtype, boundary);
            }
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var i = 0;
            foreach (var item in this)
            {
                i++;
                if (item.Stream != null)
                {
                    var content = new StreamContent(item.Stream);
                    if (item.ContentType != null)
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(item.ContentType);
                    }
                    if (item.FileName != null)
                    {
                        var contentDisposition = new ContentDispositionHeaderValue("form-data");
                        contentDisposition.Name = $"key_{i}";
                        contentDisposition.FileName = item.FileName;
                        contentDisposition.FileNameStar = contentDisposition.FileNameStar;
                        content.Headers.ContentDisposition = contentDisposition;
                    }
                    this.content.Add(content);
                }
            }
            context.HttpContext.Response.ContentLength = content.Headers.ContentLength;
            if (content.Headers.ContentType != null)
            {
                context.HttpContext.Response.ContentType = content.Headers.ContentType.ToString();
            }
            await content.CopyToAsync(context.HttpContext.Response.Body);
            foreach (var item in this)
            {
                if (item.Stream != null)
                {
                    item.Stream.Dispose();
                }
            }
        }
    }
}