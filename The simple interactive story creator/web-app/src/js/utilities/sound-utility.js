import { v4 } from "uuid";

export const loadSound = (event, success, reject) => {
    const files = event.target.files;

    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const sound = new Audio(URL.createObjectURL(file));
        sound.id = v4();
        sound.name = file.name;
        if (sound)
            success(sound, file);
        else reject("Error");

    }
};

export const soundToSrc = (sound) => {
    if (!sound) return "";
    if (sound instanceof Audio) return sound.src;
    if (typeof sound === "string") return sound;
    if (typeof sound === "object" && "sound" in sound)
        return soundToSrc(sound.sound);

    return "";
};