namespace admin  {
    export class UrlController {
        private readonly sourceId : string;
        private readonly destId : string;
        private readonly buttonId : string;
        
        constructor(sourceId: string, destId: string, buttonId: string) {
            this.sourceId = sourceId;
            this.destId = destId;
            this.buttonId = buttonId;
            
            this.handleClick()
        }
        
        private handleClick() {
            let buttonElement = document.getElementById(this.buttonId);
            if (buttonElement == null) {
                return;
            }
            buttonElement.addEventListener("click", this.generateUrl.bind(this));
        }
        
        private generateUrl() {
            let sourceElement = <HTMLInputElement>document.getElementById(this.sourceId);
            if (sourceElement == null){
                return;
            }
            let sourceValue = sourceElement.value; // Source value
            let generatedUrl = this.parseTextToUrl(sourceValue);
            
            let destElement = <HTMLInputElement>document.getElementById(this.destId);
            if (destElement == null) {
                return;
            }
            
            destElement.value = generatedUrl; // Save generated value to input
        }
        
        private parseTextToUrl(text: string){
            let str = text.normalize("NFD");
            str = str.replace(/\p{Diacritic}/gu, "");

            str = str.replace(/^\s+|\s+$/g, ''); // trim
            str = str.toLowerCase(); // lower case

            // remove accents, swap ñ for n, etc
            let from = "·/_,:;";
            let to = "------";
            for (let i = 0, l = from.length; i < l; i++) {
                str = str.replace(new RegExp(from.charAt(i), 'g'), to.charAt(i));
            }

            str = str.replace(/[^a-z0-9 -]/g, '') // remove invalid chars
                .replace(/\s+/g, '-') // collapse whitespace and replace by -
                .replace(/-+/g, '-'); // collapse dashes
            
            return str;
        }
    }
}