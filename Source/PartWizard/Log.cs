/*
	This file is part of Part Wizard /L Unleashed
		© 2021 Lisias T : http://lisias.net <support@lisias.net>
		© 2017-2018 LinugGuruGamer
		© 2014-2016 Eric Harris (ozraven)

	THIS FILE is licensed to you under:

		* WTFPL - http://www.wtfpl.net
			* Everyone is permitted to copy and distribute verbatim or modified
 				copies of this license document, and changing it is allowed as long
				as the name is changed.

	THIS FILE is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
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
		private static readonly Logger LOG = Logger.CreateForType<PartWizardPlugin>(1);

        private static readonly DateTime start = DateTime.Now;

        public static void Force(string msg, params object[] @params)
        {
            LOG.force(msg, @params);
        }

        public static void Error(string msg, params object[] @params)
        {
            LOG.error(msg, @params);
        }

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
            LOG.trace("{0}", Log.GetCallingMethod(2));
        }

        public static void Trace(string format, params object[] args)
        {
            LOG.trace("{0} {1}", Log.GetCallingMethod(2), string.Format(CultureInfo.InvariantCulture, format, args));
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
                LOG.trace("STYLE REPORT FOR {0}:", description);
                LOG.trace("\tname = {0}", style.name);
                LOG.trace("\tnormal.textColor = {0}", Log.ColorToRGB(style.normal.textColor));
                LOG.trace("\tonActive.textColor = {0}", Log.ColorToRGB(style.onActive.textColor));
                LOG.trace("\tonNormal.textColor = {0}", Log.ColorToRGB(style.onNormal.textColor));
                LOG.trace("\tonHover.textColor = {0}", Log.ColorToRGB(style.onHover.textColor));
                LOG.trace("END OF STYLE REPORT");
            }
            else
            {
                LOG.trace("STYLE REPORT FOR {0}: null", description);
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

            LOG.trace("SYMMETRY REPORT FOR {0}", r.name);
            LOG.trace("Root:");
            LOG.trace("\tname = {0}", r.name);
            LOG.trace("\tsymMethod = {0}", r.symMethod);
            LOG.trace("\tstackSymmetry = {0}", r.stackSymmetry);
            LOG.trace("Counterparts:");
            for(int index = 0; index < r.symmetryCounterparts.Count; index++)
            {
                Part c = r.symmetryCounterparts[index];

                LOG.trace("\t{0} name = {1}", index, c.name);
                LOG.trace("\t{0} symMethod = {1}", index, c.symMethod);
                LOG.trace("\t{0} stackSymmetry = {1}", index, c.stackSymmetry);
                LOG.trace("\t{0} children = {1}", index, c.children.Count);
            }
            LOG.trace("END OF SYMMETRY REPORT");
        }

		[Conditional("DEBUG")]
        public static void WriteTransformReport(Part part)
        {
            LOG.trace("TRANSFORM REPORT FOR {0}", part.name);
            LOG.trace("\ttransform = {0}", part.transform != null ? part.transform.name : "<null>");
            LOG.trace("\tpartTransform = {0}", part.partTransform != null ? part.partTransform.name : "<null>");
            Transform[] transforms = part.GetComponents<Transform>();
            if(transforms == null)
            {
                LOG.trace("\tTransforms: <n/a>");
            }
            else
            {
                LOG.trace("\tTransforms:");

                Log.WriteTransformReport(transforms, 2);
            }
            LOG.trace("END OF TRANSFORM REPORT");
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

                LOG.trace(reportLine.ToString());

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
