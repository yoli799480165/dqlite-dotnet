using System.Runtime.InteropServices;

namespace Dqlite
{
    public struct dqlite_node_info
    {
        public ulong id;
        public string address;
    }

    public class dqlite_node : SafeHandle
    {
        public dqlite_node() : base(IntPtr.Zero, true)
        {
        }

        internal static dqlite_node New(IntPtr p)
        {
            var h = new dqlite_node();
            h.SetHandle(p);
            return h;
        }

        protected override bool ReleaseHandle()
        {
            handle = IntPtr.Zero;
            return true;
        }

        public override bool IsInvalid => handle == IntPtr.Zero;
    }

    public delegate int delete_connect_func(IntPtr arg, string address, IntPtr fd);

    public static class NativeMethods
    {
        const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

        private const string LIB_DQLITE = "libdqlite";

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_version_number();

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_node_create(ulong id, string address, string data_dir, out IntPtr n);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern void dqlite_node_destroy(dqlite_node n);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_node_set_bind_address(dqlite_node n, string address);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern string dqlite_node_get_bind_address(dqlite_node n);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_node_set_connect_func(dqlite_node n, delete_connect_func f, IntPtr arg);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_node_set_network_latency_ms(dqlite_node n, uint milliseconds);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_node_set_failure_domain(dqlite_node n, ulong code);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_node_set_snapshot_params(dqlite_node n, uint snapshot_threshold, uint snapshot_trailing);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_node_enable_disk_mode(dqlite_node n);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_node_start(dqlite_node n);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_node_stop(dqlite_node n);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern int dqlite_node_recover_ext(dqlite_node n, dqlite_node_info[] infos, int n_info);

        [DllImport(LIB_DQLITE, ExactSpelling = true, CallingConvention = CALLING_CONVENTION)]
        public static extern string dqlite_node_errmsg(dqlite_node n);
    }
}
