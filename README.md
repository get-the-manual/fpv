# О ресурсе

Репозиторий базы знаний [FPV FAQ](https://get-the-manual.github.io/fpv/)

Данные материалы исходят из личного опыта, наблюдений в FPV сообществе. Никакой рекламы, спонсорства, все по личной инициативе для продвижения хобби в массы. Приятного чтения!

Проект базируется на генераторе [Github Pages](https://pages.github.com/)

Содержимое страниц лежит в папке `docs`. Pull request'ы по улучшению ресурса приветствуются.


Сайт собирается и публикуется автоматически (GitHub Actions, `.github/workflows/build-site.yml`) при каждом push в `main`. Локальный запуск нужен только для предварительного просмотра.

Навигация (`nav:` в `mkdocs.yml`) ведется вручную: новую страницу нужно добавить туда самостоятельно.

## Установка

```
pip install mkdocs-material
pip install mkdocs-autorefs
pip install mkdocs-git-revision-date-localized-plugin
```

## Локальный запуск

```
mkdocs serve
```