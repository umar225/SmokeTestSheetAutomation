# Boardwise App #

### Product Overview ###
Boardwise is a platform for non-executive directors find board opportunities to advance their career.
Admin platform to manage jobs, courses and resources.

### Component Overview ###
- Components: Basic components and headers, footers
- Config: Api and axios configs
- Routes: Protected and public routes
- Screens: Admin, Courses, Resources, Jobs and other different screens
- Store: It contain actions, api calls, reducers
- Styles: Different styles for both platform Boardwise and Coursewise
- Test: End to end testing available for checkout courses flow
- Utils: Different kind of supporting funtionalites available like env settings, amplitude etc

### Third-Party Integrations ###
- Stripe: Customer can subscribe to the memberships, for this purpose stripe payment gateway is used.
- Amplitude: To log different events.
- Hubspot: To book appointment with founder.
    
### Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.

Open [http://localhost:3000](http://localhost:3000) to view it in your browser. The page will reload when you make changes. You may also see any lint errors in the console.

### `npm run test`

It run the test in headless mode and tell you the results, if you want to view the test in browser you have to turn off the headless mode in test files.

### `npm run build`

Builds the app for production to the `build` folder. It correctly bundles React in production mode and optimizes the build for the best performance. The build is minified and the filenames include the hashes. Your app is ready to be deployed!


### Release Pipeline ###
We are using **Bitbucket** for build and deployment.
When we commit code in master branch, It trigger the build process and deployment process, we are using main branching strategy where we have only one branch i-e master it deploys to development environment when code is merged to master branch, for staging and production we have to manually trigger the deployment.
