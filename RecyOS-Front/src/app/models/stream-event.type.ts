import { Sort } from '@angular/material/sort';
import { PageEvent } from '@angular/material/paginator';

interface SortChangeEvent {
    type: 'sort';
    value: Sort;
}

interface PageChangeEvent {
    type: 'page';
    value: PageEvent;
}

interface FilterChangeEvent {
    type: 'filter';
    value: [any, number];
}

interface SortAndFilterChangeEvent {
    type: 'filter';
    value: [any, number, any];
}

interface InitialTrigger {
    type: 'initial';
}

export type StreamEvent =
    | SortChangeEvent
    | PageChangeEvent
    | FilterChangeEvent
    | SortAndFilterChangeEvent
    | InitialTrigger;
