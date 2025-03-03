import ClassesClient from './classes-client';
import { getClasses } from '@/lib/server/services/classes';

export default async function ClassesPage() {
  const classes = await getClasses();
  return <ClassesClient initialClasses={classes} />;
}
