//-------------------------------------------------------------------------------
// <copyright file="DefaultExtensionConfigurationSectionBehaviorFactory.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2011 bbv Software Services AG
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace bbv.Common.Bootstrapper.Configuration
{
    using bbv.Common.Bootstrapper.Configuration.Internals;

    /// <summary>
    /// Default factory for the <see cref="ExtensionConfigurationSectionBehavior"/>.
    /// </summary>
    public class DefaultExtensionConfigurationSectionBehaviorFactory : IExtensionConfigurationSectionBehaviorFactory
    {
        /// <inheritdoc />
        public IReflectExtensionProperties CreateReflectExtensionProperties()
        {
            return new ReflectExtensionPublicProperties();
        }

        /// <inheritdoc />
        public IAssignExtensionProperties CreateAssignExtensionProperties()
        {
            return new AssignExtensionProperties();
        }

        /// <inheritdoc />
        public IConsumeConfiguration CreateConsumeConfiguration(IExtension extension)
        {
            return new ConsumeConfiguration(extension);
        }

        /// <inheritdoc />
        public IHaveConfigurationSectionName CreateHaveConfigurationSectionName(IExtension extension)
        {
            return new HaveConfigurationSectionName(extension);
        }

        /// <inheritdoc />
        public ILoadConfigurationSection CreateLoadConfigurationSection(IExtension extension)
        {
            return new LoadConfigurationSection(extension);
        }

        /// <inheritdoc />
        public IHaveConversionCallbacks CreateHaveConversionCallbacks(IExtension extension)
        {
            return new HaveConversionCallbacks(extension);
        }

        /// <inheritdoc />
        public IHaveDefaultConversionCallback CreateHaveDefaultConversionCallback(IExtension extension)
        {
            return new HaveDefaultConversionCallback(extension);
        }
    }
}