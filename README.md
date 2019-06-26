# Architecture
Xamarin Forms Architecture (VIEW IN RAW)

This project is used as a template for most of my Xamarin Forms projects. To create a solution template from this project, follow the instructions below (OR just download the XFTemplate.zip file in this repo).

1. In Visual Studio, click on Project -> Export Template...

2. Follow the wizard instructions and create a project template for all projects in the solution.

3. Open the folder where the .zip templates is and unzip all the templates to subfolders.
  /Architecture/...
  /Architecture.Android/...
  /Architecture.iOS/...
  /Architecture.UWP/...
  /Architecture.Core/...

4. Create an xml file with extenstion .vstemplate with the following content.
  <VSTemplate Version="3.0.0" Type="ProjectGroup" xmlns="http://schemas.microsoft.com/developer/vstemplate/2005">
  <TemplateData>
    <Name>Xamarin.Forms Architecture</Name>
    <Description>Xamarin Forms template for Android, iOS and Core projects</Description>
    <ProjectType>CSharp</ProjectType>
    <ProjectSubType>
    </ProjectSubType>
    <SortOrder>1000</SortOrder>
    <CreateNewFolder>true</CreateNewFolder>
    <DefaultName>Architecture</DefaultName>
    <ProvideDefaultName>true</ProvideDefaultName>
    <LocationField>Enabled</LocationField>
    <EnableLocationBrowseButton>true</EnableLocationBrowseButton>
    <Icon>Solution_icon.png</Icon>
  </TemplateData>
  <TemplateContent>
    <ProjectCollection>
      <ProjectTemplateLink ProjectName="$projectname$" CopyParameters="true">
        Architecture\MyTemplate.vstemplate
      </ProjectTemplateLink>
      <ProjectTemplateLink ProjectName="$projectname$.Core" CopyParameters="true">
        Architecture.Core\MyTemplate.vstemplate
      </ProjectTemplateLink>
      <ProjectTemplateLink ProjectName="$projectname$.Android" CopyParameters="true">
        Architecture.Android\MyTemplate.vstemplate
      </ProjectTemplateLink>
      <ProjectTemplateLink ProjectName="$projectname$.iOS" CopyParameters="true">
        Architecture.iOS\MyTemplate.vstemplate
      </ProjectTemplateLink>
      <ProjectTemplateLink ProjectName="$projectname$.UWP" CopyParameters="true">
        Architecture.iOS\MyTemplate.vstemplate
      </ProjectTemplateLink>
    </ProjectCollection>
  </TemplateContent>
</VSTemplate>

5. Zip all the subfolders (that was unzipped in step 4) and the .vstemplate file. If you want an icon you can zip that together as well and set the icon tag in the xml. 
Zipped file content
  Root.vstemplate
  Solution_icon.png
  Architecture/...
  Architecture.Android/...
  Architecture.iOS/...
  Architecture.Core/...
  
6. Copy the final zipped file to Visual Studios template folder.

7. The template will appear when you choose to create a new project.

IMPORTANT!
8. When the solution is created you may wanna do a search and replace all words with Architecture and fix the references
