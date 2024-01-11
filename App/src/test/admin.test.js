import puppeteer from 'puppeteer';

const clickButton = async (button) => {
  button.evaluate((b) => b.click());
};

describe('Login', () => {
  test('get course suitability results, add a course to basket and checkout', async () => {
    const browser = await puppeteer.launch({
      headless: true,
      args: [
        '--disable-web-security',
        '--disable-gpu',
        '--disable-dev-shm-usage',
        '--disable-setuid-sandbox',
        '--no-sandbox',
      ],
    });
    const url = 'https://app.q.getboardwise.com';
    const page = await browser.newPage();
    await page.goto(`${url}${'/login'}`);
    await page.type('input[id="email"]', 'admin@getcoursewise.com');
    await page.type('input[id="password"]', '123456');
    await page.waitForTimeout(1000);
    let button = await page.waitForSelector('[id="loginBtn"]');
    await clickButton(button);
    await page.waitForTimeout(2000);
    button = await page.waitForSelector('[id="crsCategoryLink"]');
    await clickButton(button);
    await page.waitForTimeout(2000);
    button = await page.waitForSelector('[id="category0"]');
    await clickButton(button);
    await page.waitForTimeout(1000);
    button = await page.waitForSelector('[id="addNewBtn"]');
    await clickButton(button);
    await page.waitForTimeout(2000);
    await page.type('input[id="name"]', 'Test course name');
    await page.type('input[id="price"]', '10');
    await page.type('input[id="providerName"]', 'Test provider name');
    await page.type('input[id="providerPrice"]', '15');
    button = await page.waitForSelector('[id="lvl0"]');
    await clickButton(button);
    button = await page.waitForSelector(
      '[class="demo-editor rdw-editor-main"]'
    );
    await clickButton(button);
    await page.type('[data-text="true"]', 'This is test description text.');
    await page.waitForTimeout(2000);
    button = await page.waitForSelector('[id="submitCourse"]');
    await clickButton(button);
    await page.waitForTimeout(5000);

    const text = await page.$$eval(
      '.title',
      (e) => e[e.length - 1].textContent
    );
    expect(text).toEqual('Test course name');

    button = await page.$$('.projectMain');
    await clickButton(button[button.length - 1]);
    await page.waitForTimeout(3000);
    button = await page.waitForSelector('[id="deleteBtn"]');
    await clickButton(button);
    await page.waitForTimeout(2000);
    button = await page.waitForSelector('[id="confirmDelete"]');
    await clickButton(button);
    await page.waitForTimeout(3000);
    const text1 = await page.$$eval(
      '.title',
      (e) => e[e.length - 1].textContent
    );
    expect(text1).not.toEqual('Test course name');
  }, 80000);
});
