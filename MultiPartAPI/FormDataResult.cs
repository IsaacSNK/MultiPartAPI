using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace MultiPartAPI
{
    public class FormDataResult : Collection<MultipartContent>, IActionResult
    {
        private readonly System.Net.Http.MultipartContent content;

        public FormDataResult(string subtype = "form-data", string boundary = null)
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
            foreach (var item in this)
            {
                if (item.Stream != null)
                {
                    var content = new StreamContent(item.Stream);
                    if (item.ContentType != null)
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(item.ContentType);
                    }
                    if (item.FileName != null)
                    {
                        var contentDisposition = new ContentDispositionHeaderValue("attachment");
                        contentDisposition.FileName = item.FileName;
                        content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                        content.Headers.ContentDisposition.FileName = contentDisposition.FileName;
                        content.Headers.ContentDisposition.FileNameStar = contentDisposition.FileNameStar;
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