namespace web {
    export class MainController {

        constructor() {
            this.initCookie();
        }

        private initCookie() {
            const config = {
                disablePageInteraction: false,

                cookie: {
                    name: 'cc_cookie_demo1',
                },

                guiOptions: {
                    consentModal: {
                        layout: 'box wide',
                        position: 'bottom right',
                        flipButtons: false
                    },
                    preferencesModal: {
                        layout: 'box',
                        position: 'left',
                        equalWeightButtons: false,
                        flipButtons: false
                    }
                },

                onFirstConsent: function(){
                    console.log('onFirstAction fired');
                },

                onConsent: function () {
                    console.log('onConsent fired ...');
                },

                onChange: function () {
                    console.log('onChange fired ...');
                },

                categories: {
                    necessary: {
                        readOnly: true,
                        enabled: true
                    },
                    analytics: {
                        autoClear: {
                            cookies: [
                                {
                                    name: /^(_ga|_gid)/
                                }
                            ]
                        }
                    }
                },

                language: {
                    default: 'en',

                    translations: {
                        en: {
                            consentModal: {
                                title: 'Tento web používá cookies',
                                description: 'Používáme cookies, abychom Vám umožnili pohodlné prohlížení webu a díky analýze provozu webu neustále zlepšovali jeho funkce, výkon a použitelnost. Pro další info zobrazit <a href="#privacy-policy" data-cc="show-preferencesModal" class="cc__link">více informací</a>',
                                acceptAllBtn: 'Přijmout vše',
                                acceptNecessaryBtn: 'Odmítnost vše',
                                showPreferencesBtn: 'Správa nastavení',
                                closeIconLabel: 'Zavřít',
                                footer: `
                        <a href="./obchodni-podminky">Obchodní podmínky</a>
                    `
                            },
                            preferencesModal: {
                                title: 'Nastavení cookies 📢',
                                acceptAllBtn: 'Přimout vše',
                                acceptNecessaryBtn: 'Odmítnout vše',
                                savePreferencesBtn: 'Uložit nastavení',
                                closeIconLabel: 'Zavřít',
                                sections: [
                                    {
                                        description: 'Používáme cookies, abychom Vám umožnili pohodlné prohlížení webu a díky analýze provozu webu neustále zlepšovali jeho funkce, výkon a použitelnost.'
                                    }, {
                                        title: 'Nezbytné',
                                        description: 'Tyto cookies jsou potřeba, aby web správně fungoval.',
                                        linkedCategory: 'necessary'
                                    }, {
                                        title: 'Analytické',
                                        linkedCategory: 'analytics',
                                        cookieTable: {
                                            headers: {
                                                name: 'Name',
                                                domain: 'Service',
                                                description: 'Description',
                                                expiration: 'Expiration'
                                            },
                                            body: [
                                                {
                                                    name: '_ga',
                                                    domain: 'Google Analytics',
                                                    description: 'Cookie set by <a href="#das">Google Analytics</a>.',
                                                    expiration: 'Expires after 12 days'
                                                },
                                                {
                                                    name: '_gid',
                                                    domain: 'Google Analytics',
                                                    description: 'Cookie set by <a href="#das">Google Analytics</a>',
                                                    expiration: 'Session'
                                                }
                                            ]
                                        }
                                    }, {
                                        title: 'Více informací',
                                        description: 'Pro více informací nás můžete <a class="cc-link" href="./kontakt">kontaktovat</a>.',
                                    }
                                ]
                            }
                        }
                    }
                }
            };
            
            // @ts-ignore
            CookieConsent.run(config);
        }
    }
}
new web.MainController();