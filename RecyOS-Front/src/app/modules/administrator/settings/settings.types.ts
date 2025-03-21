// Types pour les contrôles et options
export type ControlType =
  | 'text'
  | 'number'
  | 'textarea'
  | 'select'
  | 'nativeSelect'
  | 'chip'
  | 'datepicker'
  | 'switch'
  | 'checkbox'
  | 'radio'
  | 'color'
  | 'file'
  | 'image'
  | 'video'
  | 'audio'
  | 'url'
  | 'email'
  | 'password'
  | 'tel'
  | 'search'
  | 'range'
  | 'progress'
  | 'rating'
  | 'toggle'
  | 'slider'
  | 'spinner'
  | 'calendar'
  | 'time'
  | 'datetime-local'
  | 'month'
  | 'week'
  | 'date';

export interface Option {
  value: string;
  label: string;
}

/**
 * Interface representing a system parameter
 */
export interface Parameter {
  id: number;
  module: string | null;
  nom: string | null;
  valeur?: string | number | boolean | Date; // ajuster selon vos besoins
  type: string | null;
  controlType: ControlType;
  createDate: string;
  updatedAt: string;
  createdBy: string | null;
  updatedBy: string | null;
  isDeleted: boolean;
  options?: Option[];
  placeholder?: string;
  prefixIcon?: string;
  suffixIcon?: string;
  label?: string;
}

/**
 * Data transfer object for Parameter
 */
export type ParameterDto = Parameter;

/**
 * Interface for creating a new parameter
 */
export type CreateParameterDto = Omit<
  Parameter,
  'id' | 'createDate' | 'updatedAt' | 'createdBy' | 'updatedBy' | 'isDeleted'
>;

/**
 * Interface for updating an existing parameter
 */
export type UpdateParameterDto = Partial<CreateParameterDto>;
