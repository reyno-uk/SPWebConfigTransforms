using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Reyno.SharePoint.SPWebConfigTransforms {

    [Guid("ED180730-8A65-4287-9520-5087B93D6E84")]
    public class WebConfigTransformJob : SPJobDefinition {

        public static readonly string JobName = "SharePointWebConfigTransforms";
        public static readonly string JobTitle = "SharePoint web.config transforms job";


        public WebConfigTransformJob() { }

        public WebConfigTransformJob(SPWebApplication webapp, string transform)
            : base(JobName, webapp, null, SPJobLockType.None) {

            // set up the title and the schedule for the job
            this.Title = JobTitle;
            this.Schedule = new SPOneTimeSchedule {
                Time = DateTime.UtcNow
            };

            // save the transform as a persisted property
            this._Transform = transform;

        }

        [Persisted]
        private string _Transform;

        public override void Execute(Guid targetInstanceId) {

            // exit if this is not a web front end server
            if (!IsWebFrontEnd)
                return;

            // get the parent web application
            var webapp = this.Parent as SPWebApplication;

            // perform the transforms
            TransformUtility.ApplyTransformInternal(webapp, _Transform);

        }


        public bool IsWebFrontEnd {
            get {

                // check for the web application service instance on this server
                return SPServer.Local.ServiceInstances.Any(x => x is SPWebServiceInstance);

            }
        }

        public static void ApplyTransform(SPWebApplication webapp, string transform) {

            // create the job
            var job = new WebConfigTransformJob(webapp, transform);

            // save it
            job.Update();

        }
    }
}
