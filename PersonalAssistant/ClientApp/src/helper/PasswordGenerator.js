export const LOWERCASELETTERS = 8;
export const UPPERCASELETTERS = 4;
export const NUMBERS = 2;
export const SPECIALCHARACTERS = 1;
export const defaultSpecialCharacters = '!@#$%^&*()_+=-~';

export class PasswordGenerator {
    constructor(specialCharacters = defaultSpecialCharacters) {
        this.lowercaseLetters = Array.from('abcdefghijklmnopqrstuvwxyz');
        this.uppercaseLetters = Array.from('ABCDEFGHIJKLMNOPQRSTUVWXYZ');
        this.numbers = Array.from('0123456789');
        this.specialCharacters = Array.from(specialCharacters);
    }
    //options use: LOWERCASELETTERS | UPPERCASELETTERS | NUMBERS | SPECIALCHARACTERS
    generatePassword(options, length) {
        let allValue = [];
        let requirementEnable = ('0000' + options.toString(2)).slice(-4);
        if (requirementEnable[0] === '1') {
            allValue = allValue.concat(this.lowercaseLetters);
        }
        if (requirementEnable[1] === '1') {
            allValue = allValue.concat(this.uppercaseLetters);
        }
        if (requirementEnable[2] === '1') {
            allValue = allValue.concat(this.numbers);
        }
        if (requirementEnable[3] === '1') {
            allValue = allValue.concat(this.specialCharacters);
        }
        if(length < 4)
            return '';
        if(allValue.length === 0)
            return '';
        let continueLoop = true;
        let password;
        while (continueLoop) {
            password = '';
            continueLoop = false;
            let set = new Set();
            for (let i = 0; i < length; i++) {
                let index = Math.floor(Math.random() * allValue.length);
                password += allValue[index];
                set.add(allValue[index]);
            }
            if (requirementEnable[0] === '1') {
                let hasLowercaseLetters = false;
                for (let i = 0; i < this.lowercaseLetters.length; i++) {
                    if (set.has(this.lowercaseLetters[i])) {
                        hasLowercaseLetters = true;
                        break;
                    }
                }
                if (!hasLowercaseLetters) {
                    continueLoop = true;
                }
            }

            if (requirementEnable[1] === '1') {
                let hasUppercaseLetters = false;
                for (let i = 0; i < this.uppercaseLetters.length; i++) {
                    if (set.has(this.uppercaseLetters[i])) {
                        hasUppercaseLetters = true;
                        break;
                    }
                }
                if (!hasUppercaseLetters) {
                    continueLoop = true;
                }
            }

            if (requirementEnable[2] === '1') {
                let hasNumbers = false;
                for (let i = 0; i < this.numbers.length; i++) {
                    if (set.has(this.numbers[i])) {
                        hasNumbers = true;
                        break;
                    }
                }
                if (!hasNumbers) {
                    continueLoop = true;
                }
            }
            if (requirementEnable[3] === '1') {
                let hasSpecialCharacters = false;
                for (let i = 0; i < this.specialCharacters.length; i++) {
                    if (set.has(this.specialCharacters[i])) {
                        hasSpecialCharacters = true;
                        break;
                    }
                }
                if (!hasSpecialCharacters) {
                    continueLoop = true;
                }
            }
        }
        return password;
    }
}
