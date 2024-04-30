import { createI18n } from 'vue-i18n';
import { en } from './en';
import { uk } from './uk';


export const defaultLocale = "en";

let _i18n;

const messages = {
    en: en,
    uk: uk
};

const setup = (options = { locale: defaultLocale }) => {

    _i18n = createI18n({

        locale: options.locale,
        fallbackLocale: defaultLocale,
        messages: messages
    });

    setLocale(options.locale);

    return _i18n;
};


const setLocale = (newLocale) => {
    if (messages[newLocale])
        _i18n.global.locale = newLocale;
};
const getLocale = () => {
    return _i18n.global.locale;
};

export default {

    get vueI18n() {
        return _i18n
    },
    setup,
    setLocale,
    getLocale
};

export const supportedLocales = {
    "en": { name: "English", flag: "flag-icon-gb" },
    "uk": { name: "Українська", flag: "flag-icon-ua" }
};