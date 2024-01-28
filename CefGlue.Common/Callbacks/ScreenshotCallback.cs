using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Xilium.CefGlue.Common.Callbacks
{
    internal class DevToolsReponse
    {
        public int id { get; set; }
        public DevToolsResponsePayload result { get; set; }
    }

    internal class DevToolsResponsePayload
    {
        public string data { get; set; }
    }

    internal class ScreenshotCallback : CefDevToolsMessageObserver
    {
        public byte[] ImageData { get; private set; }

        protected override void OnDevToolsAgentAttached(CefBrowser browser)
        {
        }

        protected override void OnDevToolsAgentDetached(CefBrowser browser)
        {
        }

        protected override void OnDevToolsEvent(CefBrowser browser, string method, IntPtr parameters, int parametersSize)
        {
        }

        protected override bool OnDevToolsMessage(CefBrowser browser, IntPtr message, int messageSize)
        {
            byte[] managedArray = new byte[messageSize];
            Marshal.Copy(message, managedArray, 0, messageSize);

            var responseString = Encoding.ASCII.GetString(managedArray);
            var response = System.Text.Json.JsonSerializer.Deserialize<DevToolsReponse>(responseString);
            var imageData = Convert.FromBase64String(response.result.data);
            ImageData = imageData;

            File.WriteAllBytes("screenshot.png", imageData);

            return true;
        }

        protected override void OnDevToolsMethodResult(CefBrowser browser, int messageId, bool success, IntPtr result, int resultSize)
        {
        }
    }
}
