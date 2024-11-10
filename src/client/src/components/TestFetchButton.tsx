"use client";

export default function TestFetchButton() {
  const fetchData = async () => {
    console.log("fetching data");
    const res = await fetch("/api/system/ping", { method: "GET" });
    if (res.ok) {
      console.log(await res.json());
    } else {
      console.log(res.statusText, res.status);
    }
  };
  return (
    <button type="button" onClick={fetchData}>
      フェッチ！
    </button>
  );
}
