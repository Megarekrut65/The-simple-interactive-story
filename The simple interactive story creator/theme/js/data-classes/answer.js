export const constructAnswer = (text, next, nextId, sound, action) =>{
    return{
        text: text,
        next: next,
        nextId: nextId,
        sound: sound,
        action: action
    };
};