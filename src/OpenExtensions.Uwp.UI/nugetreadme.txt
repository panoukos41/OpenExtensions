Thanks for installing OpenExtansions.Uwp.UI!

If you want to use some of the UWP-Styles-Library themes at: 
then add UwpStylesLibrary as Application resources in App.xaml: https://github.com/Raamakrishnan/UWP-Styles-Library

    <Application>
        <Application.Resources>
            <UwpStylesLibrary xmlns="using:OpenExtensions.Uwp.UI.Themes"/>
        </Application.Resources>
    </Application>

or if you have other resources then add UwpStylesLibrary a merged dictionary:

    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
				<!-- Other merged dictionaries here -->
                <UwpStylesLibrary xmlns="using:OpenExtensions.Uwp.UI.Themes"/>
                <!-- Other merged dictionaries here -->
            </ResourceDictionary.MergedDictionaries>
            <!-- Other app resources here -->
        </ResourceDictionary>
    </Application.Resources>

This is an alpha version so some things might break.