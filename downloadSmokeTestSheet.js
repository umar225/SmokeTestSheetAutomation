const { google } = require('googleapis');
const { GoogleAuth } = require('google-auth-library');
const fs = require('fs');
const xlsx = require('xlsx');


const SPREADSHEET_ID = process.env.SPREADSHEET_ID;

const RANGE = 'Boardwisesmoketest!A1:Z1000'; // Modify the range as needed

async function downloadSheet() {

    // Authorize a client with credentials
    const authClient = await authorize();

    // Create Google Sheets API instance
    const sheets = google.sheets({ version: 'v4', auth: authClient });

    // Retrieve data from the Google Sheet
    const response = await sheets.spreadsheets.values.get({
        spreadsheetId: SPREADSHEET_ID,
        range: RANGE,
    });

    // Extract the values from the response
    const values = response.data.values;

    // Convert values to xlsx format
    const ws = xlsx.utils.aoa_to_sheet(values);
    const wb = xlsx.utils.book_new();
    xlsx.utils.book_append_sheet(wb, ws, 'BoardwiseSmokeTests');

    // Save the xlsx file
    const xlsxFile = xlsx.write(wb, { bookType: 'xlsx', type: 'buffer' });
    fs.writeFileSync('smoke_test_sheet.xlsx', xlsxFile);

    console.log('Sheet downloaded and saved in xlsx format successfully!')
}

async function authorize() {


    const client = new GoogleAuth({
        credentials: {
            client_email: process.env.G_AUTH_CLIENT_EMAIL,
            private_key: process.env.G_AUTH_PRVT_KEY.replace(/\\n/g, '\n'),
        },
        scopes: ['https://www.googleapis.com/auth/spreadsheets.readonly'],
    });


    return await client.getClient();
}

// Call the function to download the sheet
downloadSheet();
