using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LazZiya.ExpressLocalization.ResxTools
{
    /// <summary>
    /// Resx files writer
    /// </summary>
    public class ResxManager
    {
        private readonly XDocument _xd;
        
        /// <summary>
        /// Initialize new instance of resource manager
        /// </summary>
        /// <param name="TResxFileType">Type of the resource file</param>
        /// <param name="resxFolderUri">Localization resources folder path</param>
        /// <param name="culture"></param>
        public ResxManager(Type TResxFileType, string resxFolderUri ="", string culture = "", string fileExt = "resx")
        {
            TargetResourceFile = string.IsNullOrWhiteSpace(culture)
                ? Path.Combine(resxFolderUri, $"{TResxFileType.Name}.{fileExt}")
                : Path.Combine(resxFolderUri, $"{TResxFileType.Name}.{culture}.{fileExt}");

            if (!File.Exists(TargetResourceFile))
            {
                try
                {
                    var dummyFileLoc = typeof(DummyResource).Assembly.Location;
                    var dummyFile = $"{dummyFileLoc.Substring(0, dummyFileLoc.LastIndexOf('\\'))}\\ResxTools\\{nameof(DummyResource)}.resx";
                    File.Copy(dummyFile, TargetResourceFile);
                }
                catch (Exception e)
                {
                    throw new FileLoadException($"Can't load or create resource file. {e.Message}");
                }
            }

            _xd = XDocument.Load(TargetResourceFile);
        }

        /// <summary>
        /// Retrive the name of the target resource file
        /// </summary>
        public string TargetResourceFile { get; private set; }

        /// <summary>
        /// Add an element to the resource file
        /// </summary>
        /// <param name="element"></param>
        /// <param name="overWriteExistingKeys"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ResxElement element, bool overWriteExistingKeys = false)
        {
            var elmnt = await FindAsync(element.Key);
            var tsk = new TaskCompletionSource<bool>();

            if (elmnt == null)
            {
                try
                {
                    _xd.Root.Add(element.ToXElement());
                    tsk.SetResult(true);
                }
                catch (Exception e)
                {
                    tsk.SetResult(false);
                }
                return await tsk.Task;
            }

            if (elmnt != null && overWriteExistingKeys == false)
            {
                tsk.SetResult(false);
                return await tsk.Task;
            }

            if (elmnt != null && overWriteExistingKeys == true)
            {
                try
                {
                    _xd.Root.Elements("data").FirstOrDefault(x => x == elmnt).ReplaceWith(element.ToXElement());
                    tsk.SetResult(true);
                }
                catch (Exception e)
                {
                    tsk.SetResult(false);
                }
                return await tsk.Task;
            }

            // if we get here something went wront
            tsk.SetResult(false);
            return await tsk.Task;
        }

        /// <summary>
        /// Add array of elements to the resource file
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="overWriteExistingKeys"></param>
        /// <returns></returns>
        public async Task<int> AddRangeAsync(ICollection<ResxElement> elements, bool overWriteExistingKeys = false)
        {
            var total = 0;

            foreach (var e in elements.Distinct())
                if (await AddAsync(e, overWriteExistingKeys))
                    total++;

            return total;
        }

        /// <summary>
        /// Async delete a resource key from resource file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool?> DeleteAsync(string key)
        {
            var elmnt = await FindAsync(key);

            if (elmnt == null)
            {
                return null;
            }

            var tsk = new TaskCompletionSource<bool>();

            try
            {
                _xd.Root.Elements("data").FirstOrDefault(x => x == elmnt).Remove();
                tsk.SetResult(true);
            }
            catch (Exception e)
            {
                tsk.SetResult(false);
            }

            return await tsk.Task;
        }

        /// <summary>
        /// Find resource by its key value
        /// </summary>
        /// <param name="key"></param>
        /// <returns>XElement</returns>
        public async Task<XElement> FindAsync(string key)
        {
            var tsk = new TaskCompletionSource<XElement>();

            await Task.Run(() =>
            {
                var elmnt = _xd.Root.Descendants("data").FirstOrDefault(x => x.Attribute("name").Value.Equals(key, StringComparison.OrdinalIgnoreCase));

                if (elmnt == null)
                    tsk.SetResult(null);

                else
                    tsk.SetResult(elmnt);
            });

            return await tsk.Task;
        }

        /// <summary>
        /// save the resource file
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAsync()
        {
            var tsk = new TaskCompletionSource<bool>();

            try
            {
                _xd.Save(TargetResourceFile);
                tsk.SetResult(true);
            }
            catch (Exception e)
            {
                tsk.SetResult(false);
            }

            return await tsk.Task;
        }
    }
}
