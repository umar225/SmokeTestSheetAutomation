import puppeteer from 'puppeteer';

const clickButton = async (button) => {
  button.evaluate((b) => b.click());
};

// If you want to run it on browser make headless: false
describe('Home', () => {
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
    await page.goto(url);
    let button = await page.waitForSelector('[data-testid="tf-v1-popup"]');
    await clickButton(button);
    const frameHandle = await page.$('iframe[data-testid="iframe"]');
    const frame = await frameHandle.contentFrame();
    await page.waitForTimeout(6000);
    // Question No 1
    button = await frame.waitForSelector(
      '[data-value-string="01FHWG5N1JT5J17R5SWY6QZW35"]'
    );
    await clickButton(button);
    await page.waitForTimeout(1000);

    // Question No 2
    button = await frame.waitForSelector(
      '[data-value-string="01FHWG5N1JMV0VY8HZQQ1B541R"]'
    );
    await clickButton(button);
    await page.waitForTimeout(1000);

    // Question No 3
    button = await frame.waitForSelector('[aria-label="Under £50"]');
    await clickButton(button);
    // Question No 3 button click
    button = await frame.waitForSelector(
      '[id="block-214c5cff-2b61-44b8-b02a-daaf362a90c0"] button'
    );
    await clickButton(button);
    await page.waitForTimeout(1000);

    // Question No 4
    button = await frame.waitForSelector('[aria-label="Remote"]');
    await clickButton(button);
    // Question No 4 button click
    button = await frame.waitForSelector(
      '[id="block-48af3ac7-a290-4240-ad90-3b7faf3f09da"] button'
    );
    await clickButton(button);
    await page.waitForTimeout(1000);

    // Question No 5
    button = await frame.waitForSelector(
      '[data-value-string="01FHWG5N1JC5JKYRYAM1K1FKSH"]'
    );
    await clickButton(button);
    await page.waitForTimeout(1000);

    // Question No 6
    button = await frame.waitForSelector('[aria-label="Face to Face"]');
    // Question No 6 button click
    await clickButton(button);
    button = await frame.waitForSelector(
      '[id="block-3211e049-2dcd-452a-8adb-2eb3c5b009f8"] button'
    );
    await clickButton(button);
    await page.waitForTimeout(1000);

    // Question No 7
    button = await frame.waitForSelector(
      '[data-value-string="01FHWG5N1KHFP03JR65MJ0ZPE9"]'
    );
    await page.waitForTimeout(1000);

    await clickButton(button);
    // Question No 8
    button = await frame.waitForSelector(
      '[data-value-string="5012bf95-1afd-47d9-88d1-444a5dec4ad3"]'
    );
    await clickButton(button);
    await page.waitForTimeout(1000);

    // Question No 9
    button = await frame.waitForSelector(
      '[data-value-string="27019ce9-00f8-4b18-bf30-fe1de205ee16"]'
    );
    await clickButton(button);
    await page.waitForTimeout(1000);

    // Question No 10
    button = await frame.waitForSelector(
      '[data-value-string="10b8f120-c579-4862-bf3c-d2cb96dac023"]'
    );
    await clickButton(button);
    await page.waitForTimeout(1000);

    // Question No 11
    await frame.waitForSelector('[placeholder="Type your answer here..."]');
    await frame.click('[placeholder="Type your answer here..."]');
    await frame.type(
      '[placeholder="Type your answer here..."]',
      'Add Some Thing....'
    );
    await page.waitForTimeout(1000);

    // Question No 11 button click
    button = await frame.waitForSelector(
      '[id="block-8e0d35b1-884c-46ab-aa05-6b89660c7a73"] button'
    );
    await clickButton(button);

    await page.waitForTimeout(1000);
    // Enter Email
    await frame.waitForSelector('[placeholder="name@example.com"]');
    await frame.click('[placeholder="name@example.com"]');
    await frame.type(
      '[placeholder="name@example.com"]',
      'end-to-end-testing@gmail.com'
    );
    // Email Submit button
    button = await frame.waitForSelector(
      '[id="block-4a729344-b284-480e-b1fb-c3a64b1bc36c"] button'
    );
    await clickButton(button);

    await page.waitForTimeout(1000);

    // Enter Phone Number
    await frame.waitForSelector('[aria-label="Phone number input"]');
    await frame.click('[aria-label="Phone number input"]');
    await frame.type('[aria-label="Phone number input"]', '03481234567');
    // Submit Button
    button = await frame.waitForSelector(
      '[data-qa="submit-button deep-purple-submit-button"]'
    );
    await clickButton(button);
    await page.waitForSelector('.headerTextWrapper>p>b');
    await page.waitForSelector('.projectDetail');
    await page.waitForTimeout(2000);
    const addtobasket = await page.$$('.buttonStyle');
    await clickButton(addtobasket[0]);
    await page.waitForTimeout(2000);
    await page.goto(`${url}/viewbasket`);
    await page.waitForSelector('.whiteBackground');
    await page.waitForTimeout(2000);
    const proceed = await page.waitForSelector('.basketTotals button');
    await clickButton(proceed);
    await page.waitForTimeout(4000);
    await page.type('input[id="billingInputName"]', 'test');
    await page.type('input[id="billingInputAddress"]', 'Street 5');
    await page.type('input[id="billingInputCity"]', 'Islamabad');
    await page.type('input[id="billingInputZip"]', '001');
    await page.type('input[id="billingInputPhone"]', '4632870');
    await page.type('input[id="billingInputEmail"]', 'test@test.com');
    await page.waitForTimeout(5000);
    const stripeFrameHandle = await page.$(
      'iframe[title="Secure payment input frame"]'
    );
    const stripeFrame = await stripeFrameHandle.contentFrame();
    await page.waitForTimeout(2000);
    await stripeFrame.type('input[name="number"]', '4242424242424242');
    await stripeFrame.type('input[name="expiry"]', '0423');
    await stripeFrame.type('input[name="cvc"]', '001');
    await page.waitForTimeout(2000);
    const submit = await page.waitForSelector('.buttonStyle');
    await clickButton(submit);
    await page.waitForTimeout(6000);
    const text = await page.$eval('#payment-message', (e) => e.textContent);
    expect(text).toContain('Payment succeeded!');
  }, 80000);
});
