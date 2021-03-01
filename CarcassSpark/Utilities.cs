﻿using AssetStudio;
using CarcassSpark.ObjectTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CarcassSpark
{
    public class Utilities
    {
        public static Dictionary<string, ContentSource> ContentSources = new Dictionary<string, ContentSource>();


        public static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string DirectoryToVanillaContent
        {
            get
            {
                if (Settings.settings["GamePath"]?.ToString() != null)
                {
                    return Path.Combine(Settings.settings["GamePath"].ToString(), "cultistsimulator_Data\\StreamingAssets\\content\\core\\");
                }
                else
                {
                    return "\\cultistsimulator_Data\\StreamingAssets\\content\\core\\";
                }
            }
        }
        // This is the root asset bundle that contains references to all the game's assets
        // We'll figure out how to access it eventually to let us view vanilla images without ripping them first
        private static string DirectoryToVanillaAssets = "\\cultistsimulator_Data\\globalgamemanagers";
        private static AssetsManager AssetsManager = new AssetsManager();
        public static Dictionary<string, Sprite> assets = new Dictionary<string, Sprite>();
        public static ImageList ImageList = new ImageList
        {
            ImageSize = new Size(128, 128)
        };

        public static DataGridViewCellStyle DictionaryExtendStyle = new DataGridViewCellStyle();
        public static DataGridViewCellStyle DictionaryRemoveStyle = new DataGridViewCellStyle();
        public static System.Drawing.Color ListAppendColor = System.Drawing.Color.LimeGreen;
        public static System.Drawing.Color ListPrependColor = System.Drawing.Color.Aquamarine;
        public static System.Drawing.Color ListRemoveColor = System.Drawing.Color.Maroon;

        static Utilities()
        {
            DictionaryExtendStyle.BackColor = System.Drawing.Color.LimeGreen;
            DictionaryRemoveStyle.BackColor = System.Drawing.Color.Maroon;
            AssetsManager.LoadFiles(Settings.settings["GamePath"].ToString() + DirectoryToVanillaAssets);
            CollectSprites();
        }

        private static void CollectSprites()
        {
            ResourceManager resourceManager = (ResourceManager)AssetsManager.assetsFileList[0].Objects.Find(@object => @object is ResourceManager);
            List<KeyValuePair<string, PPtr<AssetStudio.Object>>> containers = resourceManager.m_Container.ToList();
            foreach (KeyValuePair<string, PPtr<AssetStudio.Object>> keyValuePair in containers)
            {
                string key = keyValuePair.Key;
                PPtr<AssetStudio.Object> valuePointer = keyValuePair.Value;
                if (valuePointer.TryGet(out AssetStudio.Object value))
                {
                    if (value is Sprite sprite)
                    {
                        assets[key] = sprite;
                        ImageList.Images.Add(key, sprite.GetImage());
                    }
                }
            }
        }

        public static string GetIdType(Guid id)
        {
            if (AspectExists(id))
            {
                return "aspect";
            }

            if (ElementExists(id))
            {
                return "element";
            }

            if (RecipeExists(id))
            {
                return "recipe";
            }

            if (DeckExists(id))
            {
                return "deck";
            }

            if (LegacyExists(id))
            {
                return "legacy";
            }

            if (EndingExists(id))
            {
                return "ending";
            }

            if (VerbExists(id))
            {
                return "verb";
            }

            return "unknown";
        }

        public static string GetIdType(string id)
        {
            if (AspectExists(id))
            {
                return "aspect";
            }

            if (ElementExists(id))
            {
                return "element";
            }

            if (RecipeExists(id))
            {
                return "recipe";
            }

            if (DeckExists(id))
            {
                return "deck";
            }

            if (LegacyExists(id))
            {
                return "legacy";
            }

            if (EndingExists(id))
            {
                return "ending";
            }

            if (VerbExists(id))
            {
                return "verb";
            }

            return "unknown";
        }

        public static Bitmap GetVanillaAspect(string id)
        {
            try
            {
                string path = "images/aspects/" + id;
                if (assets.ContainsKey(path))
                {
                    return assets[path].GetImage();
                }
                else
                {
                    return assets["images/elements/_x"].GetImage();
                }
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Asset Studio's Texture Decoder Library can not be found. Please reinstall Carcass Spark.", "Missing Libraries", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        public static bool VanillaAspectImageExists(string id)
        {
            return assets.ContainsKey("images/aspects/" + id);
        }

        public static Bitmap GetVanillaElement(string id)
        {
            try
            {
                string path = "images/elements/" + id;
                if (assets.ContainsKey(path))
                {
                    return assets[path].GetImage();
                }
                else
                {
                    return assets["images/elements/_x"].GetImage();
                }
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Asset Studio's Texture Decoder Library can not be found. Please reinstall Carcass Spark.", "Missing Libraries", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool VanillaElementImageExists(string id)
        {
            return assets.ContainsKey("images/elements/" + id);
        }

        public static Bitmap GetVanillaEnding(string id)
        {
            try
            {
                string path = "images/endings/" + id;
                if (assets.ContainsKey(path))
                {
                    return assets[path].GetImage();
                }
                else
                {
                    return assets["images/endings/despair"].GetImage();
                }
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Asset Studio's Texture Decoder Library can not be found. Please reinstall Carcass Spark.", "Missing Libraries", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool VanillaEndingImageExists(string id)
        {
            return assets.ContainsKey("images/endings/" + id);
        }

        public static Bitmap GetVanillaLegacy(string id)
        {
            try
            {
                string path = "images/legacies/" + id;
                if (assets.ContainsKey(path))
                {
                    return assets[path].GetImage();
                }
                else
                {
                    return assets["images/legacies/aspirant"].GetImage();
                }
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Asset Studio's Texture Decoder Library can not be found. Please reinstall Carcass Spark.", "Missing Libraries", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool VanillaLegacyImageExists(string id)
        {
            return assets.ContainsKey("images/legacies/" + id);
        }

        public static Bitmap GetVanillaVerb(string id)
        {
            try
            {
                string path = "images/verbs/" + id;
                if (assets.ContainsKey(path))
                {
                    return assets[path].GetImage();
                }
                else
                {
                    return assets["images/verbs/_x"].GetImage();
                }
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Asset Studio's Texture Decoder Library can not be found. Please reinstall Carcass Spark.", "Missing Libraries", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool VanillaVerbImageExists(string id)
        {
            return assets.ContainsKey("images/verbs/" + id);
        }

        public static Bitmap GetVanillaCardBack(string id)
        {
            try
            {
                string path = "images/cardbacks/" + id;
                if (assets.ContainsKey(path))
                {
                    return assets[path].GetImage();
                }
                else
                {
                    return assets["images/cardbacks/_x"].GetImage();
                }
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Asset Studio's Texture Decoder Library can not be found. Please reinstall Carcass Spark.", "Missing Libraries", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool VanillaCardBackImageExists(string id)
        {
            return assets.ContainsKey("images/cardbacks/" + id);
        }

        public static Bitmap GetVanillaBurnImage(string id)
        {
            try
            {
                string path = "images/burns/" + id;
                if (assets.ContainsKey(path))
                {
                    return assets[path].GetImage();
                }
                else
                {
                    return assets["images/burns/moon"].GetImage();
                }
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Asset Studio's Texture Decoder Library can not be found. Please reinstall Carcass Spark.", "Missing Libraries", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool VanillaBurnImageImageExists(string id)
        {
            return assets.ContainsKey("images/burns/" + id);
        }

        public static bool AspectImageExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (File.Exists(source.currentDirectory + "/images/aspects/" + id + ".png"))
                {
                    return true;
                }
                else if (source.GetName() == "Vanilla" && VanillaAspectImageExists(id))
                {
                    return VanillaAspectImageExists(id);
                }
            }
            return false;
        }

        public static Image GetAspectImage(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.AspectImageExists(id))
                {
                    return source.GetAspectImage(id);
                }
                else if (source.GetName() == "Vanilla" && VanillaAspectImageExists(id))
                {
                    return GetVanillaAspect(id);
                }
            }
            string defaultImage = DirectoryToVanillaContent + "/images/elements/_x.png";
            if (File.Exists(defaultImage))
            {
                return Image.FromFile(defaultImage);
            }

            return null;
        }

        public static bool ElementImageExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (File.Exists(source.currentDirectory + "/images/elements/" + id + ".png"))
                {
                    return true;
                }
                else if (source.GetName() == "Vanilla" && VanillaElementImageExists(id))
                {
                    return VanillaElementImageExists(id);
                }
            }
            return false;
        }

        public static Image GetElementImage(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.ElementImageExists(id))
                {
                    return source.GetElementImage(id);
                }
                else if (source.GetName() == "Vanilla" && VanillaElementImageExists(id))
                {
                    return GetVanillaElement(id);
                }
            }
            string defaultImage = DirectoryToVanillaContent + "/images/elements/_x.png";
            if (File.Exists(defaultImage))
            {
                return Image.FromFile(defaultImage);
            }

            return null;
        }

        public static bool EndingImageExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (File.Exists(source.currentDirectory + "/images/endings/" + id + ".png"))
                {
                    return true;
                }
                else if (source.GetName() == "Vanilla" && VanillaEndingImageExists(id))
                {
                    return VanillaEndingImageExists(id);
                }
            }
            return false;
        }

        public static Image GetEndingImage(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.EndingImageExists(id))
                {
                    return source.GetEndingImage(id);
                }
                else if (source.GetName() == "Vanilla" && VanillaEndingImageExists(id))
                {
                    return GetVanillaEnding(id);
                }
            }
            string defaultImage = DirectoryToVanillaContent + "/images/endings/despair.png";
            if (File.Exists(defaultImage))
            {
                return Image.FromFile(defaultImage);
            }

            return null;
        }

        public static bool LegacyImageExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (File.Exists(source.currentDirectory + "/images/legacies/" + id + ".png"))
                {
                    return true;
                }
                else if (source.GetName() == "Vanilla" && VanillaLegacyImageExists(id))
                {
                    return VanillaLegacyImageExists(id);
                }
            }
            return false;
        }

        public static Image GetLegacyImage(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.LegacyImageExists(id))
                {
                    return source.GetLegacyImage(id);
                }
                else if (source.GetName() == "Vanilla" && VanillaLegacyImageExists(id))
                {
                    return GetVanillaLegacy(id);
                }
            }
            string defaultImage = DirectoryToVanillaContent + "/images/legacies/ritual.png";
            if (File.Exists(defaultImage))
            {
                return Image.FromFile(defaultImage);
            }

            return null;
        }

        public static bool VerbImageExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (File.Exists(source.currentDirectory + "/images/verbs/" + id + ".png"))
                {
                    return true;
                }
                else if (source.GetName() == "Vanilla" && VanillaVerbImageExists(id))
                {
                    return VanillaVerbImageExists(id);
                }
            }
            return false;
        }

        public static Image GetVerbImage(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.VerbImageExists(id))
                {
                    return source.GetVerbImage(id);
                }
                else if (source.GetName() == "Vanilla" && VanillaVerbImageExists(id))
                {
                    return GetVanillaVerb(id);
                }
            }
            string defaultImage = Utilities.DirectoryToVanillaContent + "/images/verbs/_x.png";
            if (File.Exists(defaultImage))
            {
                return Image.FromFile(defaultImage);
            }

            return null;
        }

        public static bool CardBackImageExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (File.Exists(source.currentDirectory + "/images/cardbacks/" + id + ".png"))
                {
                    return true;
                }
                else if (source.GetName() == "Vanilla" && VanillaCardBackImageExists(id))
                {
                    return VanillaCardBackImageExists(id);
                }
            }
            return false;
        }

        public static Image GetCardBackImage(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.CardBackImageExists(id))
                {
                    return source.GetCardBackImage(id);
                }
                else if (source.GetName() == "Vanilla" && VanillaCardBackImageExists(id))
                {
                    return GetVanillaCardBack(id);
                }
            }
            string defaultImage = Utilities.DirectoryToVanillaContent + "/images/cardbacks/_x.png";
            if (File.Exists(defaultImage))
            {
                return Image.FromFile(defaultImage);
            }

            return null;
        }

        public static bool BurnImageExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (File.Exists(source.currentDirectory + "/images/burns/" + id + ".png"))
                {
                    return true;
                }
                else if (source.GetName() == "Vanilla" && VanillaBurnImageImageExists(id))
                {
                    return VanillaBurnImageImageExists(id);
                }
            }
            return false;
        }

        public static Image GetBurnImage(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.BurnImageExists(id))
                {
                    return source.GetBurnImage(id);
                }

                if (source.GetName() == "Vanilla" && VanillaBurnImageImageExists(id))
                {
                    return GetVanillaBurnImage(id);
                }
            }
            return null;
        }

        public static bool AspectExists(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Aspects.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AspectExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Aspects.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static Aspect GetAspect(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Aspects.Get(id) != null)
                {
                    return source.Aspects.Get(id);
                }
            }
            return null;
        }

        public static Aspect GetAspect(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Aspects.GetByName(id) != null)
                {
                    return source.Aspects.GetByName(id);
                }
            }
            return null;
        }

        public static bool ElementExists(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Elements.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ElementExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Elements.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static Element GetElement(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Elements.Get(id) != null)
                {
                    return source.Elements.Get(id);
                }
            }
            return null;
        }

        public static Element GetElement(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Elements.GetByName(id) != null)
                {
                    return source.Elements.GetByName(id);
                }
            }
            return null;
        }

        public static bool RecipeExists(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Recipes.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool RecipeExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Recipes.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static Recipe GetRecipe(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Recipes.Get(id) != null)
                {
                    return source.Recipes.Get(id);
                }
            }
            return null;
        }

        public static Recipe GetRecipe(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Recipes.GetByName(id) != null)
                {
                    return source.Recipes.GetByName(id);
                }
            }
            return null;
        }

        public static bool DeckExists(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Recipes.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DeckExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Recipes.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static Deck GetDeck(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Decks.Get(id) != null)
                {
                    return source.Decks.Get(id);
                }
            }
            return null;
        }

        public static Deck GetDeck(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Decks.GetByName(id) != null)
                {
                    return source.Decks.GetByName(id);
                }
            }
            return null;
        }

        public static bool LegacyExists(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Legacies.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool LegacyExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Legacies.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static Legacy GetLegacy(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Legacies.Get(id) != null)
                {
                    return source.Legacies.Get(id);
                }
            }
            return null;
        }

        public static Legacy GetLegacy(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Legacies.GetByName(id) != null)
                {
                    return source.Legacies.GetByName(id);
                }
            }
            return null;
        }

        public static bool EndingExists(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Endings.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool EndingExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Endings.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static Ending GetEnding(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Endings.Get(id) != null)
                {
                    return source.Endings.Get(id);
                }
            }
            return null;
        }

        public static Ending GetEnding(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Endings.GetByName(id) != null)
                {
                    return source.Endings.GetByName(id);
                }
            }
            return null;
        }

        public static bool VerbExists(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Verbs.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool VerbExists(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Verbs.Exists(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static Verb GetVerb(Guid id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Verbs.Get(id) != null)
                {
                    return source.Verbs.Get(id);
                }
            }
            return null;
        }

        public static Verb GetVerb(string id)
        {
            foreach (ContentSource source in ContentSources.Values)
            {
                if (source.Verbs.GetByName(id) != null)
                {
                    return source.Verbs.GetByName(id);
                }
            }
            return null;
        }

        #region Get List of all (Type)

        public static List<Aspect> GetAspects()
        {
            return GetGameObjects<Aspect>();
        }

        public static List<Element> GetElements()
        {
            return GetGameObjects<Element>();
        }

        public static List<Recipe> GetRecipes()
        {
            return GetGameObjects<Recipe>();
        }

        public static List<Deck> GetDecks()
        {
            return GetGameObjects<Deck>();
        }

        public static List<Legacy> GetLegacies()
        {
            return GetGameObjects<Legacy>();
        }

        public static List<Ending> GetEndings()
        {
            return GetGameObjects<Ending>();
        }

        public static List<Verb> GetVerbs()
        {
            return GetGameObjects<Verb>();
        }

        public static List<T> GetGameObjects<T>() where T : IGameObject
        {
            Dictionary<Guid, T> tmp = new Dictionary<Guid, T>();
            foreach (ContentSource source in ContentSources.Values)
            {
                foreach (KeyValuePair<Guid, T> entry in source.GetContentGroup<T>())
                {
                    if (!tmp.ContainsKey(entry.Key))
                    {
                        tmp.Add(entry.Key, entry.Value);
                    }
                    else
                    {
                        tmp[entry.Key] = entry.Value;
                    }
                }
            }
            return tmp.Count > 0 ? tmp.Values.ToList<T>() : null;
        }

        #endregion

        public static ContentSource GetContentSource(string name)
        {
            if (ContentSources.ContainsKey(name))
            {
                return ContentSources[name];
            }
            else
            {
                return null;
            }
        }

        public static string SerializeObject(object objectToSerialize)
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);
            JsonSerializer jsonSerializer = new JsonSerializer();
            using (JsonTextWriter writer = new JsonTextWriter(stringWriter))
            {
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = '\t';
                writer.Indentation = 1;
                jsonSerializer.Serialize(writer, objectToSerialize);
            }
            return stringBuilder.ToString();
        }

        public static IGameObjectViewer<T> GetViewer<T>(T gameObject, EventHandler<T> successCallback) where T : IGameObject
        {
            if (typeof(T) == typeof(Aspect))
            {
                return (IGameObjectViewer<T>) new AspectViewer(gameObject as Aspect, successCallback as EventHandler<Aspect>, null);
            }
            else if (typeof(T) == typeof(Element))
            {
                return (IGameObjectViewer<T>) new ElementViewer(gameObject as Element, successCallback as EventHandler<Element>, null);
            }
            else if (typeof(T) == typeof(Recipe))
            {
                return (IGameObjectViewer<T>) new RecipeViewer(gameObject as Recipe, successCallback as EventHandler<Recipe>, null);
            }
            else if (typeof(T) == typeof(Deck))
            {
                return (IGameObjectViewer<T>) new DeckViewer(gameObject as Deck, successCallback as EventHandler<Deck>, null);
            }
            else if (typeof(T) == typeof(Legacy))
            {
                return (IGameObjectViewer<T>) new LegacyViewer(gameObject as Legacy, successCallback as EventHandler<Legacy>, null);
            }
            else if (typeof(T) == typeof(Ending))
            {
                return (IGameObjectViewer<T>) new EndingViewer(gameObject as Ending, successCallback as EventHandler<Ending>, null);
            }
            else if (typeof(T) == typeof(Verb))
            {
                return (IGameObjectViewer<T>) new VerbViewer(gameObject as Verb, successCallback as EventHandler<Verb>, null);
            }
            else
            {
                throw new ArgumentOutOfRangeException("No viewer is defined in GetViewer for Game Object " + gameObject.GetType());
            }
        }
    }
}
