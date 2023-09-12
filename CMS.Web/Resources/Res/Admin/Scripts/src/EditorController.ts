import Quill from 'quill';
import ImageUploader from "quill-image-uploader";

import 'quill-image-uploader/dist/quill.imageUploader.min.css';
Quill.register("modules/imageUploader", ImageUploader);

namespace admin {
    // TODO: Support more than one editor per page
    // TODO: upload photo to folder
    export class EditorController {
        private editor: Quill;

        constructor() {
            this.init();

            document.querySelectorAll("input[type=submit]")[0]
                .addEventListener('click', this.handleSubmit.bind(this), false);
        }

        private init() {
            if (document.getElementById('editor-value') == null){
                return;
            }

            // Editor options
            let options = {
                modules: {
                    toolbar: [
                        [{ header: [1, 2, 3, false] }],
                        ['bold', 'italic', 'strike'],
                        [{ align: [] }],
                        ['link', 'blockquote', 'code-block'],
                        [{ list: 'ordered' }, { list: 'bullet' }],
                        ["image"],

                        ['clean']
                    ],
                    //resize: {},
                    //htmlEditButton: { debug: true, syntax: true },
                    // Upload image to folder - ideal variant
                    //imageUploader: {
                    //    upload: (file:any) => {
                    //        return new Promise((resolve:any, reject:any) => {
                    //            const formData = new FormData();
                    //            formData.append("image", file);

                    //            fetch(
                    //                "/admin/File/UploadFile",
                    //                {
                    //                    method: "POST",
                    //                    body: formData
                    //                }
                    //            )
                    //                .then(response => response.json())
                    //                .then(result => {
                    //                    console.log(result);
                    //                    resolve(result.data.url);
                    //                })
                    //                .catch(error => {
                    //                    reject("Upload failed");
                    //                    console.error("Error:", error);
                    //                });
                    //            });
                    //        }
                    //    },
                    //},
                    // Compress base64 variation
                    // imageCompressor: {
                    //     quality: 0.9,
                    //     maxWidth: 1500, // default
                    //     maxHeight: 1500, // default
                    //     imageType: 'image/png'
                    // },
                    // htmlEditButton: {
                    //     syntax: true,
                    // },
                },
                placeholder: '...',
                theme: 'snow'
            }
            this.editor = new Quill('#editor', options);
        }

        public handleSubmit() {
            //@ts-ignore
            document.getElementById('editor-value').value = this.editor.root.innerHTML;
        }
    }
}
new admin.EditorController();