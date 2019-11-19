// This file is part of the mvc567 distribution (https://github.com/codific/mvc567).
// Copyright (C) 2019 Codific Ltd.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codific.Mvc567.Common
{
    public static class MimeTypes
    {
        public static class Application
        {
            public const string AtomXml = "application/atom+xml";
            public const string AtomcatXml = "application/atomcat+xml";
            public const string Ecmascript = "application/ecmascript";
            public const string JavaArchive = "application/java-archive";
            public const string Javascript = "application/javascript";
            public const string Json = "application/json";
            public const string Mp4 = "application/mp4";
            public const string OctetStream = "application/octet-stream";
            public const string Pdf = "application/pdf";
            public const string Pkcs10 = "application/pkcs10";
            public const string Pkcs7Mime = "application/pkcs7-mime";
            public const string Pkcs7Signature = "application/pkcs7-signature";
            public const string Pkcs8 = "application/pkcs8";
            public const string Postscript = "application/postscript";
            public const string RdfXml = "application/rdf+xml";
            public const string RssXml = "application/rss+xml";
            public const string Rtf = "application/rtf";
            public const string SmilXml = "application/smil+xml";
            public const string XFontOtf = "application/x-font-otf";
            public const string XFontTtf = "application/x-font-ttf";
            public const string XFontWoff = "application/x-font-woff";
            public const string XPkcs12 = "application/x-pkcs12";
            public const string XShockwaveFlash = "application/x-shockwave-flash";
            public const string XSilverlightApp = "application/x-silverlight-app";
            public const string XhtmlXml = "application/xhtml+xml";
            public const string Xml = "application/xml";
            public const string XmlDtd = "application/xml-dtd";
            public const string XsltXml = "application/xslt+xml";
            public const string Zip = "application/zip";
        }

        public static class Audio
        {
            public const string Midi = "audio/midi";
            public const string Mp4 = "audio/mp4";
            public const string Mpeg = "audio/mpeg";
            public const string Ogg = "audio/ogg";
            public const string Webm = "audio/webm";
            public const string XAac = "audio/x-aac";
            public const string XAiff = "audio/x-aiff";
            public const string XMpegurl = "audio/x-mpegurl";
            public const string XMsWma = "audio/x-ms-wma";
            public const string XWav = "audio/x-wav";
        }

        public static class Image
        {
            public const string Bmp = "image/bmp";
            public const string Gif = "image/gif";
            public const string Jpeg = "image/jpeg";
            public const string Png = "image/png";
            public const string Svg = "image/svg+xml";
            public const string Tiff = "image/tiff";
            public const string Webp = "image/webp";
            public const string Ico = "image/x-icon";

            public static string GetContentType(string format)
            {
                var field = typeof(Image).GetField(format, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Static);
                object fieldValue = field.GetValue(null);
                return fieldValue.ToString();
            }
        }

        public static class Text
        {
            public const string Css = "text/css";
            public const string Csv = "text/csv";
            public const string Html = "text/html";
            public const string Plain = "text/plain";
            public const string RichText = "text/richtext";
            public const string Sgml = "text/sgml";
            public const string Yaml = "text/yaml";
        }

        public static class Video
        {
            public const string Threegpp = "video/3gpp";
            public const string H264 = "video/h264";
            public const string Mp4 = "video/mp4";
            public const string Mpeg = "video/mpeg";
            public const string Ogg = "video/ogg";
            public const string Quicktime = "video/quicktime";
            public const string Webm = "video/webm";
        }

        public static class Font
        {
            public const string Svg = "image/svg+xml";
            public const string Ttf = "application/x-font-ttf";
            public const string Otf = "application/x-font-opentype";
            public const string Woff = "application/font-woff";
            public const string Woff2 = "application/font-woff2";
            public const string Eot = "application/vnd.ms-fontobject";
            public const string Sfnt = "application/font-sfnt";

            public static string GetContentType(string format)
            {
                var field = typeof(Font).GetField(format, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Static);
                object fieldValue = field.GetValue(null);
                return fieldValue.ToString();
            }
        }
    }
}
