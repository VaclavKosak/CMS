namespace admin {
    export class MainController{
        constructor() {
            this.init();
        }

        private init() {
            this.tooltip();
            this.validateForm();
        }

        private tooltip(){
            let tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
            tooltipTriggerList.forEach(function (tooltipTriggerEl:any) {
                // @ts-ignore
                new bootstrap.Tooltip(tooltipTriggerEl)
            });
        }

        private validateForm(){
            // Fetch all the forms we want to apply custom Bootstrap validation styles to
            let forms = document.querySelectorAll('.needs-validation')

            // Loop over them and prevent submission
            Array.prototype.slice.call(forms)
                .forEach(function (form:any) {
                    form.addEventListener('submit', function (event:any) {
                        if (!form.checkValidity()) {
                            event.preventDefault()
                            event.stopPropagation()
                        }
                        form.classList.add('was-validated')
                    }, false)
                });
        }
    }
}