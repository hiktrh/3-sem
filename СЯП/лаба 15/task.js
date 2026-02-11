document.getElementById('validateForm').addEventListener('submit', function(event) {
    event.preventDefault();
    let isValid = true;

    document.querySelectorAll('.error-message').forEach(element => element.textContent = '');
    document.querySelectorAll('input, textarea').forEach(element => element.classList.remove('error'));

    const nameSymbols = /^[a-zA-Zа-яА-ЯёЁ]{1,20}$/;
    const emailSymbols = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    const phoneSymbols = /^\(0\d{2}\)\d{3}-\d{2}-\d{2}$/;

    const surname = document.getElementById('surname');
    if (!nameSymbols.test(surname.value)) {
        isValid = false;
        surname.classList.add('error');
        document.getElementById('surname-error').textContent = 'Фамилия должна содержать только буквы, пробелы или дефисы (не более 20 символов).';
    }

    const name = document.getElementById('name');
    if (!nameSymbols.test(name.value)) {
        isValid = false;
        name.classList.add('error');
        document.getElementById('name-error').textContent = 'Имя должно содержать только буквы, пробелы или дефисы (не более 20 символов).';
    }

    const email = document.getElementById('email');
    if (!emailSymbols.test(email.value)) {
        isValid = false;
        email.classList.add('error');
        document.getElementById('email-error').textContent = 'Введите корректный Email в формате example@mail.com.';
    }

    const phone = document.getElementById('phone');
    if (!phoneSymbols.test(phone.value)) {
        isValid = false;
        phone.classList.add('error');
        document.getElementById('phone-error').textContent = 'Телефон должен быть в формате (0XX)XXX-XX-XX.';
    }

    const about = document.getElementById('about');
    if (about.value.trim().length === 0 || about.value.trim().length > 250) {
        isValid = false;
        about.classList.add('error');
        document.getElementById('about-error').textContent = 'Поле не должно быть пустым и не превышать 250 символов.';
    }

    const course = document.querySelector('input[name="course"]:checked');
    if (!course) {
        isValid = false;
        alert('Выберите курс!');
    }

    const university = document.getElementById('university');
    if (!university.checked) {
        isValid = false;
        university.classList.add('error');
        document.getElementById('university-error').textContent = 'Необходимо подтвердить, что вы учитесь в БГТУ.';
    } else {
        document.getElementById('university-error').textContent = '';
    }

    const city = document.getElementById('city');

    if (city.value !== 'Minsk' || (course && course.value !== '3') || !university.checked) {
        const confirmAnswers = confirm('Вы уверены в выбранных ответах?');
        if (!confirmAnswers) {
            isValid = false;
        }
    }

    if (isValid) {
        alert('Форма отправлена успешно!');
    }
});