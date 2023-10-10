/**
 * 
 * @param {String} firstName 
 * @param {String} lastName 
 * @param {String} email 
 * @returns 
 */
export const constructUser = (firstName, email)=>{
    return {
        firstName:firstName,
        email:email,
        role: "user"
    };
};