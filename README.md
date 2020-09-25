# Dev.to API .Net CLI Tool

This .Net console tool allows you to interact with some parts the Dev.to API and helps with posting articles from Github repos.

## Installation

### Nuget Install

Run `dotnet tool install --global devto-dotnetcore-cli --version 1.0.0` to install the package from [nuget.org.](https://www.nuget.org/packages/devto-dotnetcore-cli/1.0.0nuget.org)

### Manual Packing & Install

1. Clone this repository locally
2. Run the following commands from the root folder of the repository

    ```
    dotnet pack

    dotnet tool install -g --add-source ./nupkg devto-dotnetcore-cli
    ```


After installing the tool by either method, access it using `devto-cli`.

## Commands

### New API Key

Saves your Dev.to API key in the tool's settings to be used with Dev.to API requests.

`devto-cli new-api-key`

**Arguments**

```
--api-key (-a): Your Dev.to API key - Required
```

[Demo]()

### List All of Your Articles

Get a list of all your Dev.to articles, published and unpublished.

`devto-cli list-all-articles`

**Arguments**

N/A

[Demo]()

### Prepare Article

Prepares a Markdown article for posting to Dev.to by replacing image links to point to GitHub repo

`devto-cli prep-article`

**Arguments**

```
--article-path (-p): The path to the article's Markdown file - Required
--github-user  (-u): Your Github username - Required
--github-repo  (-r): The name of the article's Github repo - Required
--images-path  (-i): The path within the repo to the images - Required
```

[Demo]()

### Get Article by URL

Gets any Dev.to article by using the URL of the article

`devto-cli get-article-by-url`

**Arguments**

```
--url (-u): The URL of the article - Required
```

[Demo]()

### Post Article

Posts an article to Dev.to

`devto-cli post-article`

**Arguments**

```
--title        (-t): The title of the article - Required
--article-path (-p): The path to the article's Markdown file - Required
--main-image   (-i): The URL of the image to be used as a header image of the article - Optional
--series       (-s): The name of the series the article belongs to - Optional
--published        : Choose whether to publish the article now or not - Optional
--tags             : A list of tags to attach to the article - Optional
```

[Demo]()

### Update Article

Updates an existing Dev.to article using the article's URL

`devto-cli update-article`

**Arguments**

```
--url          (-u): The URL of the article to be update - Required
--title        (-t): The title of the article - Required
--article-path (-p): The path to the article's Markdown file - Required
--main-image   (-i): The URL of the image to be used as a header image of the article - Optional
--series       (-s): The name of the series the article belongs to - Optional
--published        : Choose whether to publish the article now or not - Optional
--tags             : A list of tags to attach to the article - Optional
```

[Demo]()
