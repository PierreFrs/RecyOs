import { defineConfig } from 'cypress';
import * as fs from 'fs';
import * as path from "node:path";

const envConfig = JSON.parse(fs.readFileSync('cypress.env.json', 'utf-8'));

export default defineConfig({
    e2e: {
        baseUrl: envConfig.baseUrl || 'http://localhost:4200',
        viewportWidth: 1280,
        viewportHeight: 720,
        supportFile: 'cypress/support/index.ts',
        defaultCommandTimeout: 10000,
        chromeWebSecurity: false,
        downloadsFolder: 'cypress/downloads',
        setupNodeEvents(on) {
            on('before:browser:launch', (browser, launchOptions) => {
                if (browser.name === 'chrome') {
                    launchOptions.preferences.default['download'] = {
                        prompt_for_download: false,
                        default_directory: path.resolve(__dirname, 'cypress/downloads'),
                    };
                }

                if (browser.name === 'firefox') {
                    launchOptions.preferences['browser.download.dir'] = path.resolve(__dirname, 'cypress/downloads');
                    launchOptions.preferences['browser.download.folderList'] = 2;
                    launchOptions.preferences['browser.helperApps.neverAsk.saveToDisk'] = 'application/pdf';
                }

                return launchOptions;
            });
        },
    },
    env: envConfig,
});
