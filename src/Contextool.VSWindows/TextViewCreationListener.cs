namespace Contextool.VSWindows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Threading.Tasks;
    using Contextool.Editor.Common;
    using Contextool.VSWindows.Build;
    using Contextool.VSWindows.Common;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Threading;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(ITextViewCreationListener))]
    [Name(Name)]
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.PrimaryDocument)]
    internal sealed class TextViewCreationListener : ITextViewCreationListener
    {
        private const string Name = "Contextool TextView Creation Listener";
        private const string ConfigurationFileName = "contextool.json";
        private JoinableTaskContext joinableTaskContext;
        private ITextDocumentFactoryService textDocumentFactoryService;
        private readonly Logger logger;

        [ImportingConstructor]
        public TextViewCreationListener(
            JoinableTaskContext joinableTaskContext,
            ITextDocumentFactoryService textDocumentFactoryService,
            Logger logger)
        {
            this.joinableTaskContext = joinableTaskContext
                ?? throw new ArgumentNullException(nameof(joinableTaskContext));
            this.textDocumentFactoryService = textDocumentFactoryService
                ?? throw new ArgumentNullException(nameof(textDocumentFactoryService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void TextViewCreated(ITextView textView)
        {
            if (this.TryGetDocumentFilePath(textView.TextBuffer, out var filePath) &&
                this.TryFindConfigurationFilePathForFile(filePath, out var configurationFilePath))
            {
                // TODO: load configuration only once.
                this.joinableTaskContext.Factory.RunAsync(async delegate
                {
                    try
                    {
                        var configuration = await Configuration.ReadFromFileAsync(configurationFilePath);
                        foreach (var projectPath in configuration.Projects)
                        {
                            this.logger.LogMessage(string.Format(Strings.DiscoveredProjectMessage, projectPath));

                            var globalProperties = new Dictionary<string, string>();
                            var projectCollection = new Microsoft.Build.Evaluation.ProjectCollection(
                                globalProperties, new[] { new BuildLogger(this.logger) },
                                Microsoft.Build.Evaluation.ToolsetDefinitionLocations.Default);

                            var project = projectCollection.LoadProject(projectPath);
                            project.Build();
                        }
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogException(ex);
                    }

                });
            }
            else
            {
                this.logger.LogMessage(
                    string.Format(
                        Strings.ConfigurationNotFoundFormat,
                        ConfigurationFileName,
                        filePath));
            }
        }

        private bool TryFindConfigurationFilePathForFile(string filePath, out string configurationFilePath)
        {
            while (true)
            {
                filePath = Path.GetDirectoryName(filePath);
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    configurationFilePath = string.Empty;
                    return false;
                }

                configurationFilePath = Path.Combine(filePath, ConfigurationFileName);
                if (File.Exists(configurationFilePath))
                {
                    return true;
                }
            }
        }

        private bool TryGetDocumentFilePath(ITextBuffer textBuffer, out string filePath)
        {
            if (this.textDocumentFactoryService.TryGetTextDocument(textBuffer, out var document))
            {
                filePath = document.FilePath;
                return true;
            }

            filePath = string.Empty;
            return false;
        }
    }
}
