// Copyright (c) 2014, Eric Harris (ozraven)
// All rights reserved.

// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the copyright holder nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.

// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL ERIC HARRIS BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using UnityEngine;

using Logger = KSPe.Util.Log.Logger; // To solve conflict with UnityEngine's one

namespace PartWizard
{
    internal static class Log
    {
		private static readonly Logger LOG = Logger.CreateForType<PartWizardPlugin>();

        private static readonly DateTime start = DateTime.Now;

		[Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
            if(!condition)
            {
                string message = string.Format(CultureInfo.InvariantCulture, "Assertion failed in {0}.", Log.GetCallingMethod(2));
#if TEST
                throw new Exception(message);
#endif
                LOG.info(message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[Conditional("DEBUG")]
        public static void Assert(bool condition, string format, params object[] args)
        {
            if(!condition)
            {
                string message = string.Format(CultureInfo.InvariantCulture, "Assertion failed in {0}: {1}", Log.GetCallingMethod(2), string.Format(CultureInfo.InvariantCulture, format, args));
#if TEST
                throw new Exception(message);
#endif
                LOG.info(message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void Trace()
        {
            LOG.info("{0}", Log.GetCallingMethod(2));
        }

        public static void Trace(string format, params object[] args)
        {
            LOG.info("{0} {1}", Log.GetCallingMethod(2), string.Format(CultureInfo.InvariantCulture, format, args));
        }

        private static string GetCallingMethod(int skipCount)
        {
            StackFrame stackFrame = new StackFrame(skipCount);

            MethodBase method = stackFrame.GetMethod();

            return string.Concat(method.DeclaringType, ".", method.Name);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static string Format(Rect rect)
        {
            return string.Format(CultureInfo.InvariantCulture, "[({0}, {1}) {2}x{3}]", (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static string FormatInt32(Vector2 vector)
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1})", (int)vector.x, (int)vector.y);
        }

		[Conditional("DEBUG")]
        public static void WriteStyleReport(GUIStyle style, string description)
        {
            if(style != null)
            {
                LOG.info("STYLE REPORT FOR {0}:", description);
                LOG.info("\tname = {0}", style.name);
                LOG.info("\tnormal.textColor = {0}", Log.ColorToRGB(style.normal.textColor));
                LOG.info("\tonActive.textColor = {0}", Log.ColorToRGB(style.onActive.textColor));
                LOG.info("\tonNormal.textColor = {0}", Log.ColorToRGB(style.onNormal.textColor));
                LOG.info("\tonHover.textColor = {0}", Log.ColorToRGB(style.onHover.textColor));
                LOG.info("END OF STYLE REPORT");
            }
            else
            {
                LOG.info("STYLE REPORT FOR {0}: null", description);
            }
        }

        private static string ColorToRGB(Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", (byte)(Mathf.Clamp01(color.r)), (byte)(Mathf.Clamp01(color.g)), (byte)(Mathf.Clamp01(color.b)));
        }

		[Conditional("DEBUG")]
        public static void WriteSymmetryReport(Part part)
        {
            Part r = PartWizard.FindSymmetryRoot(part);

            LOG.info("SYMMETRY REPORT FOR {0}", r.name);
            LOG.info("Root:");
            LOG.info("\tname = {0}", r.name);
            LOG.info("\tsymMethod = {0}", r.symMethod);
            LOG.info("\tstackSymmetry = {0}", r.stackSymmetry);
            LOG.info("Counterparts:");
            for(int index = 0; index < r.symmetryCounterparts.Count; index++)
            {
                Part c = r.symmetryCounterparts[index];

                LOG.info("\t{0} name = {1}", index, c.name);
                LOG.info("\t{0} symMethod = {1}", index, c.symMethod);
                LOG.info("\t{0} stackSymmetry = {1}", index, c.stackSymmetry);
                LOG.info("\t{0} children = {1}", index, c.children.Count);
            }
            LOG.info("END OF SYMMETRY REPORT");
        }

		[Conditional("DEBUG")]
        public static void WriteTransformReport(Part part)
        {
            LOG.info("TRANSFORM REPORT FOR {0}", part.name);
            LOG.info("\ttransform = {0}", part.transform != null ? part.transform.name : "<null>");
            LOG.info("\tpartTransform = {0}", part.partTransform != null ? part.partTransform.name : "<null>");
            Transform[] transforms = part.GetComponents<Transform>();
            if(transforms == null)
            {
                LOG.info("\tTransforms: <n/a>");
            }
            else
            {
                LOG.info("\tTransforms:");

                Log.WriteTransformReport(transforms, 2);
            }
            LOG.info("END OF TRANSFORM REPORT");
        }

		[Conditional("DEBUG")]
        private static void WriteTransformReport(Transform[] transforms, int tabCount)
        {
            for(int transformIndex = 0; transformIndex < transforms.Length; transformIndex++)
            {
                StringBuilder reportLine = new StringBuilder();

                for(int tabIndex = 0; tabIndex < tabCount; tabIndex++)
                {
                    reportLine.Append("\t");
                }

                Transform transform = transforms[transformIndex];

                reportLine.AppendFormat("{0} name = {1} ({2} children)", transformIndex, transform.name, transform.childCount);

                LOG.info(reportLine.ToString());

                if(transform.childCount > 0)
                {
                    Log.WriteTransformReport(transform.GetChildren(), tabCount + 1);
                }
            }
        }

		internal static void Write(string msg, params object[] @params)
		{
			LOG.info(msg, @params);
		}

		private static Transform[] GetChildren(this Transform transform)
        {
#if DEBUG
            Transform[] result = new Transform[transform.childCount];

            for(int index = 0; index < transform.childCount; index++)
            {
                result[index] = transform.GetChild(index);
            }

            return result;
#else
            return new Transform[0];
#endif // DEBUG
        }
    }
}
