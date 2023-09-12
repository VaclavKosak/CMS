namespace admin {
    export class GalleryController {
        private readonly imageFormId:string;
        private readonly urlElementId:string;
        
        private progressBar:HTMLDivElement;

        constructor(imageFormId:string, urlElementId:string) {
            this.imageFormId = imageFormId;
            this.urlElementId = urlElementId;

            this.init();
        }

        private init() {
            document.addEventListener('submit', this.UploadImages.bind(this));
            
            this.progressBar = <HTMLDivElement>document.getElementById('progress-bar');
        }

        public async UploadImages(event:any) {
            event.preventDefault();
            event.stopPropagation();
            event.stopImmediatePropagation();
            let formElement = <HTMLFormElement>document.getElementById(this.imageFormId);
            if (formElement == null) {
                return;
            }
            const formData = new FormData(formElement)

            // GET URL
            let parentUrlElement = <HTMLInputElement>document.getElementById(this.urlElementId);
            if (parentUrlElement == null){
                return;
            }
            let parentUrl = parentUrlElement.value;

            let url = formElement.action + "/" + parentUrl;
            
            let maxValue = formData.getAll("Files.FileUpload").length;
            let valueCounter = 0;
            
            for (const file of formData.getAll("Files.FileUpload")) {
                await admin.GalleryController.SendRequest(url, file).then(() => this.UpdateProgressBarValue(maxValue, valueCounter));
                valueCounter++;
            }

            location.reload();
        }

        private static async SendRequest(url:string, file:any)
        {
            const formData = new FormData()
            formData.append("file", file);

            try {
                const response = await fetch(url, {
                    method: 'POST',
                    body: formData
                });

                if (response.ok) {
                    return true;
                }
            } catch (error) {
                return false;
            }
        }
        
        private UpdateProgressBarValue(maxValue:number, actualValue:number)
        {
            let value = Math.round(actualValue*100/maxValue);
            this.progressBar.setAttribute("style", `width:${value}%`);
            this.progressBar.innerText = `${value}%`;
        }
    }
}
new admin.GalleryController("imageForm", "url");