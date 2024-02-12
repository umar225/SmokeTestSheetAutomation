# Continuous Integration for Smoke Test Sheet

## Description

This project automates the process of updating the smoke test sheet used for testing purposes by integrating it with CI/CD pipelines. The smoke test sheet is regularly synchronized with the latest data from a Google Sheet, ensuring that the testing process remains up-to-date and aligned with the development cycle.

## Files

1. **downloadSmokeTestSheet.js:**
   - This Node.js script is responsible for downloading the latest data from a specified Google Sheet and saving it as an Excel file (`smoke_test_sheet.xlsx`).

2. **bitbucket-pipelines.yml:**
   - This YAML file defines the Bitbucket Pipelines configuration, orchestrating the CI/CD process for updating the smoke test sheet.
   - It includes steps for installing dependencies, checking the existence of the existing smoke test sheet, downloading the latest data, and committing the changes back to the repository.

## Prerequisites

- Node.js installed on your machine.
- Bitbucket repository configured with Pipelines enabled.
- Google Cloud project with the Google Sheets API enabled.
- Service account credentials (JSON file) with appropriate permissions to access the Google Sheet.

## Configuration

1. Obtain a service account key file (JSON) from the Google Cloud Console and save it securely.
2. Set up environment variables in your Bitbucket repository for:
   - `SPREADSHEET_ID`: The ID of the Google Sheet to sync with.
   - `G_AUTH_CLIENT_EMAIL`: Client email from the service account key file.
   - `G_AUTH_PRVT_KEY`: Private key from the service account key file.

## Usage

1. Ensure your Google Sheet is shared with the service account email.
2. Push changes to your Bitbucket repository.
3. Bitbucket Pipelines will automatically trigger the CI/CD process defined in `bitbucket-pipelines.yml`, updating the smoke test sheet accordingly.

## Contributing

Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch (`git checkout -b feature`).
3. Make your changes.
4. Commit your changes (`git commit -am 'Added feature'`).
5. Push to the branch (`git push origin feature`).
6. Create a new Pull Request.
