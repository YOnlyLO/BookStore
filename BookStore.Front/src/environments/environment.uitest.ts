// Окружение UI-тестов (BookStore.WebUiTest): тесты поднимают собственный API с временной базой
// на отдельном порту, чтобы не мешать запущенному для разработки экземпляру.
export const environment = {
  apiUrl: 'http://localhost:5299'
};
