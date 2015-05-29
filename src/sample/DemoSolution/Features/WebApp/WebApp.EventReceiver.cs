using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Reyno.SharePoint.SPWebConfigTransforms;

namespace DemoSolution.Features.WebApp {
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("4098d2ef-acf0-4669-8452-056abb59173e")]
    public class WebAppEventReceiver : SPFeatureReceiver {

        public override void FeatureActivated(SPFeatureReceiverProperties properties) {

            ApplyTransforms(properties, "WebConfigTransforms\\web.apply.config");

        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties) {

            ApplyTransforms(properties, "WebConfigTransforms\\web.remove.config");

        }

        private void ApplyTransforms(SPFeatureReceiverProperties properties, string path) {

            // get the web application for this activation
            var webapp = properties.Feature.Parent as SPWebApplication;

            // get the path to the transform file
            var transformFile = Path.Combine(
                properties.Definition.RootDirectory,
                path
                );

            // read the contents of the transform file
            var transform = File.ReadAllText(transformFile);

            // apply the transform
            TransformUtility.ApplyTransform(webapp, transform);

        }
    


    }
}
