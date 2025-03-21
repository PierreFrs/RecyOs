import {UserDto} from "../../../modules/administrator/users/users.type";

export interface Notification
{
    id: string;
    icon?: string;
    image?: string;
    title?: string;
    description?: string;
    time: string;
    link?: string;
    useRouter?: boolean;
    read: boolean;
    user: UserDto;
    userId: number;
}
