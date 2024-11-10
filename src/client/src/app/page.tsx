import TestFetchButton from "@/components/TestFetchButton";
import styles from "./page.module.css";

export default function Home() {
  return (
    <div className={styles.page}>
      <main className={styles.main}>
        <div>TEST</div>
        <div>
          <TestFetchButton/>
        </div>
      </main>
    </div>
  );
}
