const fontNames = [
    "HyperBrush",
    "HyperSchlag-ExtraBold",
    "Lato-Regular",
    "MathiasRegular",
    "Minecraft",
    "Playball"
]

const createFonts = ()=>{
    const fonts = [];

    for(let i in fontNames){
        fonts.push({name:fontNames[i], path:`resources/fonts/${fontNames[i]}.ttf`})
    }

    fonts.unshift({name:"Arial", path:undefined});
    
    return fonts;
};

export const FONTS = createFonts();