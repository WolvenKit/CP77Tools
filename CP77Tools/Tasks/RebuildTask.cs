﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Catel.IoC;
using CP77.Common.Services;
using CP77.Common.Tools.FNV1A;
using CP77.CR2W;
using CP77.CR2W.Archive;
using CP77.CR2W.Types;
using WolvenKit.Common.Tools.DDS;

namespace CP77Tools.Tasks
{

    public static partial class ConsoleFunctions
    {
        /// <summary>
        /// Recombine split buffers and textures in a folder.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="outpath"></param>
        public static void RebuildTask(string[] path, 
            bool buffers, 
            bool textures,
            bool import,
            bool keep,
            bool clean,
            bool unsaferaw
            )
        {
            if (path == null || path.Length < 1)
            {
                logger.LogString("Please fill in an input path", Logtype.Error);
                return;
            }

            Parallel.ForEach(path, p =>
            {
                RebuildTaskInner(p, buffers, textures, import, keep, clean, unsaferaw);
            });

        }

        private static void RebuildTaskInner(string path, 
            bool buffers, 
            bool textures,
            bool import,
            bool keep,
            bool clean,
            bool unsaferaw
        )
        {
            #region checks

            if (string.IsNullOrEmpty(path))
            {
                logger.LogString("Please fill in an input path", Logtype.Error);
                return;
            }

            var inputDirInfo = new DirectoryInfo(path);
            if (!Directory.Exists(path) || !inputDirInfo.Exists)
            {
                logger.LogString("Input path does not exist", Logtype.Error);
                return;
            }

            var basedir = inputDirInfo;
            if (basedir?.Parent == null) return;

            #endregion


            ModTools.Recombine(basedir, buffers, textures, import, keep, clean, unsaferaw);

            return;
        }
    }
}