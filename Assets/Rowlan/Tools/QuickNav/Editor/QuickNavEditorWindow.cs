using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rowlan.Tools.QuickNav
{
    public class QuickNavEditorWindow : EditorWindow
    {
        [MenuItem(ProjectSetup.MENU)]
        static void CreateWindow()
        {
            QuickNavEditorWindow wnd = EditorWindow.GetWindow<QuickNavEditorWindow>();
            wnd.titleContent.text = ProjectSetup.WINDOW_TITLE;

            wnd.position = new Rect(QuickNavSettingsProvider.WindowPositionX, QuickNavSettingsProvider.WindowPositionY, QuickNavSettingsProvider.WindowWidth, QuickNavSettingsProvider.WindowHeight);
            //wnd.minSize = new Vector2(400,600);
            //wnd.Close();
        }

        public enum QuickNavTab
        {
            History,
            Favorites
        }

        private GUIContent[] quickNavTabs;
        private int selectedQuickNavTabIndex = 0;

        private QuickNavEditorWindow editorWindow;

        private QuickNavDataManager dataManager = new QuickNavDataManager();

        private QuickNavEditorModule historyModule;
        private QuickNavEditorModule favoritesModule;

        void OnEnable()
        {
            editorWindow = this;

            // initialize data
            dataManager.OnEnable();

            // get the selected tab from the data manager
            // this ensures the selected tab keeps selected during unity sessions
            // it would change to the editor preferences if the user adds or removes any script
            selectedQuickNavTabIndex = dataManager.quickNavData.selectedTabIndex;

            if(selectedQuickNavTabIndex < 0)
            {
                selectedQuickNavTabIndex = QuickNavSettingsProvider.InitialTab;
            }

            // unity startup, first access
            if ( !Startup.Instance.Initialized)
            {
                // check startup or play mode: don't do anything when the user switches to play mode in the editor
                bool isUnityStartup = EditorApplication.isPlayingOrWillChangePlaymode == false;

                if (isUnityStartup)
                {
                    // clear history at startup
                    dataManager.InitializeHistory();
                }
            }

            // update history and favorites using the object guid
            // this may become necessary after a restart of the editor
            dataManager.Refresh();


            #region Modules

            // TODO: add data manager to module instead of others, remove reference to editorwindow
            // history
            historyModule = new QuickNavEditorModule( dataManager, QuickNavEditorModule.ModuleType.History)
            {
                headerText = "History",
                reorderEnabled = false,
                addSelectedEnabled = false,
            };
            historyModule.OnEnable();

            // favorites
            favoritesModule = new QuickNavEditorModule( dataManager, QuickNavEditorModule.ModuleType.Favorites)
            {
                headerText = "Favorites",
                reorderEnabled = true,
                addSelectedEnabled = true,
            };
            favoritesModule.OnEnable();

            #endregion Modules

            quickNavTabs = new GUIContent[]
            {
                new GUIContent( QuickNavTab.History.ToString()),
                new GUIContent( QuickNavTab.Favorites.ToString()),
            };

            // initialize selection
            OnSelectionChange();

            Startup.Instance.Initialized = true;

            // hook into the scene change for refresh of the objects when another scene gets loaded
            UnityEditor.SceneManagement.EditorSceneManager.sceneOpened -= SceneOpenedCallback;
            UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += SceneOpenedCallback;
        }

        void OnDisable()
        {
            // remove scene change hook
            UnityEditor.SceneManagement.EditorSceneManager.sceneOpened -= SceneOpenedCallback;
        }

        /// <summary>
        /// Scene change handler
        /// </summary>
        /// <param name="_scene"></param>
        /// <param name="_mode"></param>
        void SceneOpenedCallback(Scene _scene, UnityEditor.SceneManagement.OpenSceneMode _mode)
        {
            // update history and favorites using the object guid
            // this may become necessary after a restart of the editor
            dataManager.Refresh();
        }


        void OnGUI()
        {
            dataManager.GetSerializedObject().Update();

            selectedQuickNavTabIndex = GUILayout.Toolbar(selectedQuickNavTabIndex, quickNavTabs, GUILayout.Height(20));

            // save the currently selected tab index
            if (dataManager.quickNavData.selectedTabIndex != selectedQuickNavTabIndex)
            {
                dataManager.quickNavData.selectedTabIndex = selectedQuickNavTabIndex;
            }

            switch (selectedQuickNavTabIndex)
            {
                case ((int)QuickNavTab.History):
                    historyModule.OnGUI();
                    break;

                case ((int)QuickNavTab.Favorites):
                    favoritesModule.OnGUI();
                    break;

            }

            dataManager.GetSerializedObject().ApplyModifiedProperties();
        }

        void OnInspectorUpdate()
        {
            // Call Repaint on OnInspectorUpdate as it repaints the windows
            // less times as if it was OnGUI/Update
            Repaint();
        }

        /// <summary>
        /// Analyze selection, add to history
        /// </summary>
        private void OnSelectionChange()
        {
            // skip adding to history if the new selected one is the current selected one;
            // this would be the case for the jump function
            if (historyModule.CurrentSelectionMatchesFirstItem())
                return;

            // add currently selected to history
            dataManager.AddSelectedToHistory();
        }

    }
}