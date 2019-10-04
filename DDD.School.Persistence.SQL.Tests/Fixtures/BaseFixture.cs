using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DDD.School.Persistence.SQL.Tests.Fixtures
{
    public abstract class BaseFixture
    {
        protected BaseFixture()
        {
            SetEnvironmentVariables();

            this.EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;

            Console.WriteLine($"running on environment {this.EnvironmentName}");
        }

        public string EnvironmentName { get; }

        private static void SetEnvironmentVariables()
        {
            var launchSettingsJson = Path.Combine("Properties", "launchSettings.json");
            if (!File.Exists(launchSettingsJson))
                return;

            using var file = File.OpenText(launchSettingsJson);
            using var reader = new JsonTextReader(file);
            var variables = JObject.Load(reader)
                .GetValue("profiles")
                .SelectMany(profiles => profiles.Children())
                .SelectMany(profile => profile.Children<JProperty>())
                .Where(prop => prop.Name == "environmentVariables")
                .SelectMany(prop => prop.Value.Children<JProperty>())
                .ToList();
            foreach (var variable in variables)
            {
                var value = Environment.GetEnvironmentVariable(variable.Name) ?? variable.Value.ToString();
                Environment.SetEnvironmentVariable(variable.Name, value);
            }
        }
    }
}
