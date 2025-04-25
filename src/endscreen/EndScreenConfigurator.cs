using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace EndScreen;

public static class EndScreenConfigurator
{
    public static void ConfigureEndScreen(string pathToChromeUserData, string[] videoIds)
    {
        string profile = "Default"; // Or "Profile 1", "Profile 2", etc.

        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument($"--user-data-dir={pathToChromeUserData}");
        options.AddArgument($"--profile-directory={profile}");

        using var driver = new ChromeDriver(options);

        foreach (var id in videoIds)
        {
            Console.WriteLine($"Configuring video: {id}");
            if (string.IsNullOrWhiteSpace(id)) continue;

            string trimmedId = id.Trim();
            string editUrl = $"https://studio.youtube.com/video/{trimmedId}/edit";
            Console.WriteLine($"Opening: {editUrl}");
            driver.Navigate().GoToUrl(editUrl);

            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // ✅ Wait for and click the "End screen" button
                var endScreenButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id=\"endscreen-editor-link\"]/ytcp-dropdown-trigger/div/div[2]/span")
                ));
                endScreenButton.Click();

                // ✅ Wait for the end screen modal to appear
                wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//div[contains(text(), 'Import from video')]")
                ));

                // ✅ Wait for and click the "1 video + 1 subscribe" template
                var template = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id=\"cards-row\"]/div[1]/div[1]")
                ));
                template.Click();

                // ✅ Wait briefly (UI animation delay)
                Thread.Sleep(1000);

                // ✅ Wait 1 second before clicking "Save"
                Thread.Sleep(1000);

                // ✅ Click the "Save" button (without waiting for its visibility)
                var saveButton = driver.FindElement(By.XPath("//*[@id='save-button']//button[contains(@aria-label, 'Save')]"));
                saveButton.Click();

                Thread.Sleep(10000);

                Console.WriteLine("✅ End screen applied and dialog disappeared.");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("⏳ Timed out waiting for an element. Skipping...");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("⚠️ Could not locate an expected element. Skipping...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error applying end screen: {ex.Message}");
            }
        }

        Console.WriteLine("Done.");
    }
}
