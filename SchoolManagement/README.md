# School Management System

A web application for managing students, teachers, and classes built with ASP.NET Core, Blazor, and Sekiban Event Sourcing.

## Viewport Scaling Feature

This application includes a viewport scaling feature that allows you to adjust the size of the user interface to better fit your screen resolution. By default, the UI is scaled to 66% of its original size.

### How to Toggle Viewport Scaling

There are several ways to enable or disable viewport scaling:

#### 1. Using Keyboard Shortcut

Press `Ctrl + Alt + S` to toggle viewport scaling on or off.

#### 2. Using JavaScript Console

Open the browser's developer console (F12 or Ctrl+Shift+I) and use one of these functions:

```javascript
// Enable scaling
enableViewportScaling();

// Disable scaling
disableViewportScaling();

// Toggle scaling
toggleViewportScaling();

// Check if scaling is enabled
isViewportScalingEnabled();
```

#### 3. Editing the HTML

To permanently disable viewport scaling, you can edit the `App.razor` file and comment out or remove the viewport-scale.css link:

```html
<!-- Viewport Scaling - Comment out or remove this line to disable scaling -->
<link rel="stylesheet" href="@Assets["viewport-scale.css"]" />
```

> **Note:** If you disable viewport scaling by editing the HTML, you'll need to rebuild and restart the application for the changes to take effect.

### Customizing the Scaling Factor

If you want to use a different scaling factor than 66%, you can edit the `viewport-scale.css` file and change the scale values. See the [viewport scaling help page](SchoolManagement.Web/wwwroot/viewport-scaling-help.html) for detailed instructions.

### Help Documentation

For more detailed information about the viewport scaling feature, visit the help page at `/viewport-scaling-help.html` when the application is running.

## Running the Application

To run the application:

```bash
cd SchoolManagement/SchoolManagement.AppHost
dotnet run --launch-profile https
```

The web frontend will be available at https://localhost:7201.

## Project Structure

- **SchoolManagement.Domain**: Contains domain models, events, commands, and queries
- **SchoolManagement.ApiService**: API endpoints for commands and queries
- **SchoolManagement.Web**: Web frontend with Blazor
- **SchoolManagement.AppHost**: Aspire host for orchestrating services
- **SchoolManagement.ServiceDefaults**: Common service configurations
