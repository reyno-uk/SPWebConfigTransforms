using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SharePoint.Administration;
using Microsoft.Web.XmlTransform;

namespace Reyno.SharePoint.SPWebConfigTransforms {

    public class TransformUtility {
        
        public static void ApplyTransform(SPWebApplication webapp, string transform) {
            
            // create the job to update all web.config files on all front end web servers
            WebConfigTransformJob.ApplyTransform(webapp, transform);

        }

        internal static void ApplyTransformInternal(SPWebApplication webapp, string transform) {

            var webConfigs = GetWebConfigs(webapp);

            foreach (var path in webConfigs) {

                ApplyTransformInternal(path, transform);

            }


        }

        private static void ApplyTransformInternal(string path, string transform) {


            // setup
            var transformation = GetXmlTransformation(transform);
            var document = GetTransformableDocument(path);

            // apply the requested transform
            transformation.Apply(document);

            // save the changed document
            document.Save(path);


        }

        private static XmlTransformableDocument GetTransformableDocument(string path) {

            var doc = new XmlTransformableDocument {
                PreserveWhitespace = true
            };

            doc.Load(path);

            return doc;

        }

        private static XmlTransformation GetXmlTransformation(string transform) {

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream)) {

                writer.Write(transform);
                writer.Flush();

                stream.Position = 0;

                return new XmlTransformation(stream, null);

            }

        }


        private static IEnumerable<string> GetWebConfigs(SPWebApplication webapp) {

            var webconfigs =
                from setting in webapp.IisSettings
                from file in setting.Value.Path.EnumerateFiles("web.config")
                select file.FullName;

            return webconfigs;

        }



    }
}
