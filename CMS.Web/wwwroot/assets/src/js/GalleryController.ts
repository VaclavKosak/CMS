namespace web {
    export class GalleryController {
        private readonly imageFormId:string;
        private readonly urlElementId:string;
        
        constructor(imageFormId:string, urlElementId:string) {
            this.imageFormId = imageFormId;
            this.urlElementId = urlElementId;
            
            this.init();
        }

        private init() {
            document.addEventListener('submit', this.UploadImages.bind(this));
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
            
            try {
                const response = await fetch(url, {
                    method: 'POST',
                    body: formData
                });

                if (response.ok) {
                    location.reload();
                }
            } catch (error) {
                console.error('Error:', error);
            }
        }
    }
}
