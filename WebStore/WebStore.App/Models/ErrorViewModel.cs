using System;

namespace WebStore.App.Models
{
    public class ErrorViewModel
    {
        public string Message { get; set; }

        public string StoreName { get; set; }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}