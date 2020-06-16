using LazZiya.ExpressLocalization.ResxTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LazZiya.ExpressLocalization.Json
{
    /// <summary>
    /// Resx files writer
    /// </summary>
    public class JsonManager
    {
        private readonly XDocument _xd;

        /// <summary>
        /// Initialize new instance of json manager
        /// </summary>
        public JsonManager(Type TResxFileType, string jsonFolderUri = "", string culture = "")
        {
            TargetJsonFile = string.IsNullOrWhiteSpace(culture)
                ? Path.Combine(jsonFolderUri, $"{TResxFileType.Name}.json")
                : Path.Combine(jsonFolderUri, $"{TResxFileType.Name}.{culture}.json");

            _xd = XDocument.Load(TargetJsonFile);
        }

        /// <summary>
        /// Retrive the name of the target resource file
        /// </summary>
        public string TargetJsonFile { get; private set; }

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
                _xd.Save(TargetJsonFile);
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
