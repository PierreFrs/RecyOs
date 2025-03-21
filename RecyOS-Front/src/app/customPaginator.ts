import {MatPaginatorIntl} from "@angular/material/paginator";


export function CustomPaginator() {
    const customPaginatorIntl = new MatPaginatorIntl();

    customPaginatorIntl.itemsPerPageLabel = 'Nombre d\'éléments par page:';
    customPaginatorIntl.firstPageLabel = 'Première page';
    customPaginatorIntl.lastPageLabel = 'Dernière page';
    customPaginatorIntl.nextPageLabel = 'Page suivante';
    customPaginatorIntl.previousPageLabel = 'Page précédente';

    return customPaginatorIntl;
}
